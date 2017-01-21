//-----------------------------------------------------------------------
// Copyright 2014 Tobii Technology AB. All rights reserved.
//-----------------------------------------------------------------------

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System;
using Tobii.EyeX.Client;

namespace Tobii.EyeTracking
{
    /// <summary>
    /// Holds an engine state value and a flag indicating the validity of the value.
    /// </summary>
    /// <typeparam name="T">Data type of the engine state.</typeparam>
    public sealed class StateValue<T> : EventArgs, IStateValue<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateValue{T}"/> class.
        /// </summary>
        /// <param name="value">The state handler.</param>
        public StateValue(T value)
        {
            Value = value;
            IsValid = true;
        }

        private StateValue()
        {
            // Will result in a value where IsValid is false
        }

        /// <summary>
        /// Gets a value representing an invalid state value.
        /// </summary>
        public static StateValue<T> Invalid
        {
            get
            {
                return new StateValue<T>();
            }
        }

        /// <summary>
        /// Gets the state value.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the state value is valid.
        /// The state will always be unknown when disconnected from Tobii Engine.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            if (IsValid)
            {
                if (typeof(T) == typeof(Size2))
                {
                    var value = (Size2)((object)Value);
                    return string.Format("{0:0.0} x {1:0.0}", value.Width, value.Height);
                }

                if (typeof(T) == typeof(Rect))
                {
                    var value = (Rect)((object)Value);
                    return string.Format("({0}, {1}), {2} x {3}", value.X, value.Y, value.Width, value.Height);
                }

                return Value.ToString();
            }
            else
            {
                return "INVALID";
            }
        }
    }
}

#else
namespace Tobii.EyeTracking
{
    public sealed class StateValue<T> : StateValueStub<T>
    {
        public StateValue(T value) : base(value)
        {
        }
    }
}
#endif
