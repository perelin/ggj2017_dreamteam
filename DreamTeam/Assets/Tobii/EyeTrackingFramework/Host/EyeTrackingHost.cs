//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using System.Collections;
using System.Collections.Generic;
using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;
using UnityEngine;
using Environment = Tobii.EyeX.Client.Environment;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Provides the main point of contact with Tobii Engine. 
    /// </summary>
    [AddComponentMenu("")]
    public partial class EyeTrackingHost : MonoBehaviour, IEyeTrackingHost
    {
        /// <summary>
        /// If set to true, it will automatically initialize Tobii Engine on Start().
        /// </summary>
        public bool initializeOnStart = true;

        private static bool _isShuttingDown;

        private static EyeTrackingHost _instance;

        private readonly object _lock = new object();
        private readonly Dictionary<string, IDataProviderInternal> _dataProviders = new Dictionary<string, IDataProviderInternal>();
        private Environment _environment;
        private Context _context;
        private GameViewInfo _gameViewInfo = new GameViewInfo(new Vector2(float.NaN, float.NaN), Vector2.one);
        private float _frameTimestamp = float.NaN;
        private bool _isConnected;
        private bool _isPaused;
        private bool _runInBackground;
        private GameViewBoundsProvider _gameViewBoundsProvider;
        private Version _engineVersion;
        private GazeFocus _gazeFocus;

        // Engine state accessors
        private StateAccessor<Tobii.EyeX.Client.Rect> _screenBoundsStateAccessor;
        private StateAccessor<Tobii.EyeX.Client.Size2> _displaySizeStateAccessor;
        private StateAccessor<EyeTrackingDeviceStatus> _eyeTrackingDeviceStatusStateAccessor;
        private StateAccessor<Tobii.EyeX.Framework.UserPresence> _userPresenceStateAccessor;
        private StateAccessor<Tobii.EyeX.Framework.GazeTracking> _gazeTracking;

        /// <summary>
        /// Gets the GazeFocus handler.
        /// </summary>
        public IGazeFocus GazeFocus
        {
            get { return _gazeFocus; }
        }

        //--------------------------------------------------------------------
        // States
        //--------------------------------------------------------------------

        /// <summary>
        /// Gets the engine state: Screen bounds in pixels.
        /// </summary>
        public IStateValue<UnityEngine.Rect> ScreenBounds
        {
            get
            {
                return EnumHelpers.ConvertFromEyeXRect(
                    _screenBoundsStateAccessor.GetCurrentValue(_context));
            }
        }

        /// <summary>
        /// Gets the engine state: Display size as Vector2(width, height), in millimeters.
        /// </summary>
        public IStateValue<Vector2> DisplaySize
        {
            get
            {
                return EnumHelpers.ConvertFromEyeXSize2(
                    _displaySizeStateAccessor.GetCurrentValue(_context));
            }
        }

        /// <summary>
        /// Gets the engine state: Eye tracking status.
        /// </summary>
        public DeviceStatus EyeTrackingDeviceStatus
        {
            get
            {
                return EnumHelpers.ConvertFromEyeXDeviceStatus(
                    _eyeTrackingDeviceStatusStateAccessor.GetCurrentValue(_context));
            }
        }

        /// <summary>
        /// Gets the engine state: User presence.
        /// </summary>
        public UserPresence UserPresence
        {
            get
            {
                return new UserPresence(
                    EnumHelpers.ConvertFromEyeXUserPresence(
                    _userPresenceStateAccessor.GetCurrentValue(_context)));
            }
        }

        /// <summary>
        /// Gets the engine state: Gaze tracking.
        /// </summary>
        /// <value>The gaze tracking.</value>
        public GazeTracking GazeTracking
        {
            get
            {
                return new GazeTracking(
                    EnumHelpers.ConvertFromEyeXGazeTracking(
                    this, _gazeTracking.GetCurrentValue(_context)));
            }
        }

        //--------------------------------------------------------------------
        // Properties
        //--------------------------------------------------------------------

        /// <summary>
        /// Gets the engine state: Gaze tracking.
        /// </summary>
        /// <value>The gaze tracking.</value>
        public Version EngineVersion
        {
            get { return _engineVersion; }
        }

        /// <summary>
        /// Returns a value indicating whether the EyeTracking host has been initialized
        /// </summary>
        public bool IsInitialized
        {
            get { return _context != null; }
        }

        /// <summary>
        /// Gets a value indicating whether the host is running.
        /// </summary>
        /// <value><c>true</c> if the host is running; otherwise, <c>false</c>.</value>
        private bool IsRunning
        {
            get
            {
                return !_isPaused || _runInBackground;
            }
        }

        //--------------------------------------------------------------------
        // Singleton Accessor
        //--------------------------------------------------------------------

        /// <summary>
        /// Gets the singleton EyeTrackingHost instance.
        /// Always use this method to access the EyeTrackingHost
        /// </summary>
        /// <returns>The instance.</returns>
        public static IEyeTrackingHost GetInstance()
        {
            if (_isShuttingDown)
            {
                return new EyeTrackingHostStub();
            }

            if (_instance == null)
            {
                // create a game object with a new instance of this class attached as a component.
                // (there's no need to keep a reference to the game object, because game objects are not garbage collected.)
                var container = new GameObject("EyeTrackingHostContainer");
                DontDestroyOnLoad(container);
                _instance = (EyeTrackingHost)container.AddComponent(typeof(EyeTrackingHost));
            }

            return _instance;
        }

        //--------------------------------------------------------------------
        // MonoBehaviour event functions (messages)
        //--------------------------------------------------------------------

        /// <summary>
        /// Initialize helper classes and state accessors on Awake
        /// </summary>
        void Awake()
        {
            _runInBackground = Application.runInBackground;

#if UNITY_EDITOR
            _gameViewBoundsProvider = CreateEditorScreenHelper();
#else
        _gameViewBoundsProvider = new UnityPlayerGameViewBoundsProvider();
#endif
            _gazeFocus = new GazeFocus();

            _screenBoundsStateAccessor = new StateAccessor<Tobii.EyeX.Client.Rect>(StatePaths.EyeTrackingScreenBounds);
            _displaySizeStateAccessor = new StateAccessor<Size2>(StatePaths.EyeTrackingDisplaySize);
            _eyeTrackingDeviceStatusStateAccessor = new StateAccessor<EyeTrackingDeviceStatus>(StatePaths.EyeTrackingState);
            _userPresenceStateAccessor = new StateAccessor<Tobii.EyeX.Framework.UserPresence>(StatePaths.UserPresence);
            _userProfileNameStateAccessor = new StateAccessor<string>(StatePaths.ProfileName);
            _userProfileNamesStateAccessor = new EnumerableStateAccessor<string>(StatePaths.EyeTrackingProfiles);
            _gazeTracking = new StateAccessor<Tobii.EyeX.Framework.GazeTracking>(StatePaths.GazeTracking);
        }

#if UNITY_EDITOR
        private static GameViewBoundsProvider CreateEditorScreenHelper()
        {
#if UNITY_4_5 || UNITY_4_3 || UNITY_4_2 || UNITY_4_1
        return new LegacyEditorGameViewBoundsProvider();
#else
            return new EditorGameViewBoundsProvider();
#endif
        }
#endif

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            if (initializeOnStart)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            _frameTimestamp = Time.time;

            if (_engineVersion == null && IsInitialized && _isConnected)
            {
                _engineVersion = GetEngineVersion();
            }

            // update the gameView Position, in case the game window has been moved or resized.
            var gameViewBounds = _gameViewBoundsProvider.GetGameViewPhysicalBounds();
            var gameViewPosition = new Vector2(gameViewBounds.x, gameViewBounds.y);
            var gameViewPixelsPerDesktopPixel = new Vector2(Screen.width / gameViewBounds.width, Screen.height / gameViewBounds.height);
            _gameViewInfo = new GameViewInfo(gameViewPosition, gameViewPixelsPerDesktopPixel);

            _gazeFocus.UpdateGazeFocus();

            StartCoroutine(DoEndOfFrameCleanup());
        }

        private IEnumerator DoEndOfFrameCleanup()
        {
            yield return new WaitForEndOfFrame();
            lock (_lock)
            {
                foreach (var dataProvider in _dataProviders.Values)
                {
                    dataProvider.EndFrame();
                }
            }
        }

        /// <summary>
        /// Sent to all game objects when the player pauses.
        /// </summary>
        /// <param name="pauseStatus">Gets a value indicating whether the player is paused.</param>
        void OnApplicationPause(bool pauseStatus)
        {
            var wasRunning = IsRunning;
            _isPaused = pauseStatus;

            // make sure that data streams are disabled while the game is paused.
            if (wasRunning != IsRunning && _isConnected)
            {
                CommitAllGlobalInteractors();
            }
        }

        /// <summary>
        /// Sent to all game objects before the application is quit.
        /// </summary>
        void OnApplicationQuit()
        {
            Shutdown();
        }

        //--------------------------------------------------------------------
        // Data Streams
        //--------------------------------------------------------------------

        /// <summary>
        /// Gets a provider of gaze point data using default data processing.
        /// </summary>
        /// <returns>The data provider.</returns>
        public IDataProvider<GazePoint> GetGazePointDataProvider()
        {
            return GetGazePointDataProvider(GazePointDataMode.LightlyFiltered);
        }

        /// <summary>
        /// EXPERIMENTAL
        /// Gets a provider of eye position data.
        /// See <see cref="IDataProvider{T}"/>.
        /// </summary>
        /// <returns>The data provider.</returns>
        /// <remarks>
        /// This data stream should be considered experimental. It might be
        /// removed or changed in a future release. We have not been able
        /// to find any kind of proper or good-enough user experience based 
        /// on this data but provide it here for you to experiment with.
        /// </remarks>
        public IDataProvider<EyePositions> GetEyePositionDataProvider()
        {
            var dataStream = new EyePositionDataProvider();
            return GetDataProviderForDataStream<EyePositions>(dataStream);
        }

        /// <summary>
        /// Gets a provider of gaze point data.
        /// See <see cref="IDataProvider{T}"/>.
        /// </summary>
        /// <param name="mode">Specifies the kind of data processing to be applied by Tobii Engine.</param>
        /// <returns>The data provider.</returns>
        private IDataProvider<GazePoint> GetGazePointDataProvider(GazePointDataMode mode)
        {
            var dataStream = new GazePointDataProvider(mode);
            return GetDataProviderForDataStream<GazePoint>(dataStream);
        }

        /// <summary>
        /// Gets a provider of fixation data.
        /// See <see cref="IDataProvider{T}"/>.
        /// </summary>
        /// <param name="mode">Specifies the kind of data processing to be applied by Tobii Engine.</param>
        /// <returns>The data provider.</returns>
        private IDataProvider<FixationPoint> GetFixationDataProvider(FixationDataMode mode)
        {
            var dataStream = new FixationDataProvider(mode);
            return GetDataProviderForDataStream<FixationPoint>(dataStream);
        }

        /// <summary>
        /// Gets a data provider for a given data stream: preferably an existing one 
        /// in the _globalInteractors collection, or, failing that, the one passed 
        /// in as a parameter.
        /// </summary>
        /// <typeparam name="T">Type of the provided data value object.</typeparam>
        /// <param name="dataStream">Data stream to be added.</param>
        /// <returns>A data provider.</returns>
        private IDataProvider<T> GetDataProviderForDataStream<T>(DataProviderBase<T> dataStream) where T : ITimestamped
        {
            lock (_lock)
            {
                IDataProviderInternal existing;
                if (_dataProviders.TryGetValue(dataStream.Id, out existing))
                {
                    return (IDataProvider<T>)existing;
                }

                _dataProviders.Add(dataStream.Id, dataStream);
                dataStream.Updated += OnGlobalInteractorUpdated;
                return dataStream;
            }
        }

        /// <summary>
        /// Gets an interactor from the repository.
        /// </summary>
        /// <param name="interactorId">ID of the interactor.</param>
        /// <returns>Interactor, or null if not found.</returns>
        private IDataProviderInternal GetGlobalInteractor(string interactorId)
        {
            lock (_lock)
            {
                IDataProviderInternal interactor = null;
                _dataProviders.TryGetValue(interactorId, out interactor);
                return interactor;
            }
        }

        //--------------------------------------------------------------------
        // Lifecycle Methods
        //--------------------------------------------------------------------

        /// <summary>
        /// Initializes the EyeTracking host and connection to Tobii Engine.
        /// </summary>
        public void Initialize()
        {
            if (IsInitialized) return;

            try
            {
                Tobii.EyeX.Client.Interop.EyeX.EnableMonoCallbacks("mono");
                _environment = Environment.Initialize();
            }
            catch (InteractionApiException ex)
            {
                Debug.LogError("Tobii EyeTracking initialization failed: " + ex.Message);
            }
            catch (DllNotFoundException)
            {
#if UNITY_EDITOR
                Debug.LogError("Tobii EyeTracking initialization failed because the client access library 'Tobii.EyeX.Client.dll' could not be loaded. " +
                    "Please make sure that it is present in the Unity project directory. " +
                    "You can find it in the SDK package, in the lib/x86 directory. (Currently only Windows is supported.)");
#else
			Debug.LogError("Tobii EyeTracking initialization failed because the client access library 'Tobii.EyeX.Client.dll' could not be loaded. " +
				"Please make sure that it is present in the root directory of the game/application.");
#endif
                return;
            }

            try
            {
                _context = new Context(false);
                _context.RegisterEventHandler(HandleEvent);
                _context.ConnectionStateChanged += OnConnectionStateChanged;
                _context.EnableConnection();

                print("Tobii EyeTracking is running.");
            }
            catch (InteractionApiException ex)
            {
                Debug.LogError("Tobii Engine context initialization failed: " + ex.Message);
            }
        }

        /// <summary>
        /// Shuts down the EyeTracking host.
        /// </summary>
        public void Shutdown()
        {
            _isShuttingDown = true;

            if (!IsInitialized) return;
            print("Tobii EyeTracking is shutting down.");

            if (_context != null)
            {
                // The context must be shut down before disposing.
                try
                {
                    _context.Shutdown(1000, false);
                }
                catch (InteractionApiException ex)
                {
                    Debug.LogError("Tobii Engine context shutdown failed: " + ex.Message);
                }

                _context.Dispose();
                _context = null;
            }

            if (_environment != null)
            {
                _environment.Dispose();
                _environment = null;
            }
        }

        //--------------------------------------------------------------------
        // Internal Methods
        //--------------------------------------------------------------------

        internal GameViewInfo GetGameViewInfo()
        {
            return _gameViewInfo;
        }

        //--------------------------------------------------------------------
        // Private Methods
        //--------------------------------------------------------------------

        private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            if (e.State == ConnectionState.Connected)
            {
                _isConnected = true;

                // commit the snapshot with the global interactor as soon as the connection to the engine is established.
                // (it cannot be done earlier because committing means "send to the engine".)
                CommitAllGlobalInteractors();

                _screenBoundsStateAccessor.OnConnected(_context);
                _displaySizeStateAccessor.OnConnected(_context);
                _eyeTrackingDeviceStatusStateAccessor.OnConnected(_context);
                _userPresenceStateAccessor.OnConnected(_context);
                _userProfileNameStateAccessor.OnConnected(_context);
                _userProfileNamesStateAccessor.OnConnected(_context);
                _gazeTracking.OnConnected(_context);
            }
            else
            {
                _isConnected = false;

                _screenBoundsStateAccessor.OnDisconnected();
                _displaySizeStateAccessor.OnDisconnected();
                _eyeTrackingDeviceStatusStateAccessor.OnDisconnected();
                _userPresenceStateAccessor.OnDisconnected();
                _userProfileNameStateAccessor.OnDisconnected();
                _userProfileNamesStateAccessor.OnDisconnected();
                _gazeTracking.OnDisconnected();
            }
        }

        private void HandleEvent(InteractionEvent event_)
        {
            // NOTE: this method is called from a worker thread, so it must not access any game objects.
            using (event_)
            {
                try
                {
                    // Route the event to the appropriate interactor, if any.
                    var interactorId = event_.InteractorId;
                    var globalInteractor = GetGlobalInteractor(interactorId);
                    if (globalInteractor != null)
                    {
                        globalInteractor.HandleEvent(event_, _frameTimestamp, _gameViewInfo);
                    }
                }
                catch (InteractionApiException ex)
                {
                    print("Tobii Engine event handler failed: " + ex.Message);
                }
            }
        }

        private void OnGlobalInteractorUpdated(object sender, EventArgs e)
        {
            var globalInteractor = (IDataProviderInternal)sender;

            if (_isConnected)
            {
                CommitGlobalInteractors(new[] { globalInteractor });
            }
        }

        private void CommitAllGlobalInteractors()
        {
            // make a copy of the collection of interactors to avoid race conditions.
            List<IDataProviderInternal> globalInteractorsCopy;
            lock (_lock)
            {
                if (_dataProviders.Count == 0) { return; }

                globalInteractorsCopy = new List<IDataProviderInternal>(_dataProviders.Values);
            }

            CommitGlobalInteractors(globalInteractorsCopy);
        }

        private void CommitGlobalInteractors(IEnumerable<IDataProviderInternal> globalInteractors)
        {
            try
            {
                var snapshot = CreateGlobalInteractorSnapshot();
                var forceDeletion = !IsRunning;
                foreach (var globalInteractor in globalInteractors)
                {
                    globalInteractor.AddToSnapshot(snapshot, forceDeletion);
                }

                CommitSnapshot(snapshot);
            }
            catch (InteractionApiException ex)
            {
                print("Tobii Engine operation failed: " + ex.Message);
            }
        }

        private Snapshot CreateGlobalInteractorSnapshot()
        {
            var snapshot = _context.CreateSnapshot();
            snapshot.CreateBounds(BoundsType.None);
            snapshot.AddWindowId(Literals.GlobalInteractorWindowId);
            return snapshot;
        }

        private void CommitSnapshot(Snapshot snapshot)
        {
#if DEVELOPMENT_BUILD
		snapshot.CommitAsync(OnSnapshotCommitted);
#else
            snapshot.CommitAsync(null);
#endif
        }

#if DEVELOPMENT_BUILD
	private static void OnSnapshotCommitted(AsyncData asyncData)
	{
		try
		{
			ResultCode resultCode;
			if (!asyncData.TryGetResultCode(out resultCode)) { return; }

			if (resultCode == ResultCode.InvalidSnapshot)
			{
				print("Snapshot validation failed: " + GetErrorMessage(asyncData));
			}
			else if (resultCode != ResultCode.Ok && resultCode != ResultCode.Cancelled)
			{
				print("Could not commit snapshot: " + GetErrorMessage(asyncData));
			}
		}
		catch (InteractionApiException ex)
		{
			print("EyeTracking operation failed: " + ex.Message);
		}

		asyncData.Dispose();
	}

	private static string GetErrorMessage(AsyncData asyncData)
	{
		string errorMessage;
		if (asyncData.Data.TryGetPropertyValue<string>(Literals.ErrorMessage, out errorMessage))
		{
			return errorMessage;
		}
		else
		{
			return "Unspecified error.";
		}
	}
#endif

        private Version GetEngineVersion()
        {
            if (!IsInitialized)
            {
                throw new InvalidOperationException("Tobii EyeTracking is not initialized.");
            }

            if (_engineVersion != null)
            {
                return _engineVersion;
            }

            var stateBag = _context.GetState(StatePaths.EngineVersion);
            string value;
            if (!stateBag.TryGetStateValue(out value, StatePaths.EngineVersion))
            {
                throw new InvalidOperationException("Could not get engine version.");
            }
            return new Version(value);
        }
    }
}

#else
namespace Tobii.EyeTracking
{
    public partial class EyeTrackingHost : EyeTrackingHostStub
    {
        // all implementation in the stub
    }
}
#endif