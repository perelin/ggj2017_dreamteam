//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using Tobii.EyeX.Client;
using Tobii.EyeX.Framework;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Accesses and monitors engine states.
    /// Used by the EyeTrackingHost.
    /// </summary>
    /// <typeparam name="T">Data type of the engine state.</typeparam>
    internal class StateAccessor<T>
    {
        private readonly string _statePath;
        private readonly AsyncDataHandler _handler;
        private IStateValue<T> _currentValue;
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateAccessor{T}"/> class.
        /// </summary>
        /// <param name="statePath">The state path.</param>
        public StateAccessor(string statePath)
        {
            _statePath = statePath;
            _handler = OnStateChanged;
        }

        /// <summary>
        /// Gets the state path.
        /// </summary>
        /// <value>The state path.</value>
        public string StatePath
        {
            get { return _statePath; }
        }

        /// <summary>
        /// Gets the current value of the engine state.
        /// </summary>
        /// <param name="context">The interaction context.</param>
        /// <returns>The state value.</returns>
        public IStateValue<T> GetCurrentValue(Context context)
        {
            if (_currentValue == null)
            {
                // This is the first time someone asks for the current value.
                // We don't have a value yet, but we'll make sure we get one.
                _currentValue = StateValue<T>.Invalid;

                // Register the state changed handler.
                RegisterStateChangedHandler(context);
            }

            return _currentValue;
        }

        /// <summary>
        /// Registers a listener for state-changed events.
        /// </summary>
        /// <param name="context">The interaction context.</param>
        private void RegisterStateChangedHandler(Context context)
        {
            if (!_isInitialized && context != null)
            {
                // When the first event listener is registered: register a state-changed handler with the context.
                context.RegisterStateChangedHandler(_statePath, _handler);
                context.GetStateAsync(_statePath, _handler);

                // We're now initialized.
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Method to be invoked when a connection to Tobii Engine has been established.
        /// </summary>
        /// <param name="context">The interaction context.</param>
        public void OnConnected(Context context)
        {
            if (_isInitialized)
            {
                // When connected: send a request for the initial state.
                context.GetStateAsync(_statePath, _handler);
            }
        }

        /// <summary>
        /// Method to be invoked when the connection to Tobii Engine has been lost.
        /// </summary>
        public void OnDisconnected()
        {
            if (_isInitialized)
            {
                // When disconnected: raise a state-changed event marking the state as invalid.
                _currentValue = StateValue<T>.Invalid;
            }
        }

        /// <summary>
        /// Gets the data from the state bag.
        /// </summary>
        /// <param name="bag">The bag.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if data could be retrieved; otherwise <c>false</c>.</returns>
        protected virtual bool GetData(StateBag bag, out T value)
        {
            return bag.TryGetStateValue(out value, _statePath);
        }

        private void OnStateChanged(AsyncData data)
        {
            using (data)
            {
                ResultCode resultCode;
                if (!data.TryGetResultCode(out resultCode) || resultCode != ResultCode.Ok)
                {
                    return;
                }

                using (var stateBag = data.GetDataAs<StateBag>())
                {
                    if (stateBag != null)
                    {
                        if (_isInitialized)
                        {
                            T value;
                            if (GetData(stateBag, out value))
                            {
                                _currentValue = new StateValue<T>(value);
                            }
                        }
                    }
                }
            }
        }
    }
}
#endif
