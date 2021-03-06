﻿using System;
using System.Threading.Tasks;
using IO.Ably;
using IO.Ably.Types;

namespace IO.Ably.Transport.States.Connection
{
    using IO.Ably.Realtime;

    internal class ConnectionClosingState : ConnectionStateBase
    {
        private const int CloseTimeout = 1000;
        private readonly ICountdownTimer _timer;

        /// <summary>
        /// used to mitigate a potential race condition where by OnAttachToContext()
        /// can be called after Connect() is called but before the new state is attached
        /// </summary>
        private bool _inConnectTransition = false;

        public ConnectionClosingState(IConnectionContext context, ILogger logger)
            : this(context, null, new CountdownTimer("Closing state timer", logger), logger)
        {
        }

        public ConnectionClosingState(IConnectionContext context, ErrorInfo error, ICountdownTimer timer, ILogger logger)
            : base(context, logger)
        {
            _timer = timer;
            Error = error ?? ErrorInfo.ReasonClosed;
        }

        public override ConnectionState State => Realtime.ConnectionState.Closing;

        public override Task<bool> OnMessageReceived(ProtocolMessage message)
        {
            switch (message.Action)
            {
                case ProtocolMessage.MessageAction.Closed:
                    TransitionState(new ConnectionClosedState(Context, Logger));
                    return TaskConstants.BooleanTrue;
                case ProtocolMessage.MessageAction.Disconnected:
                    TransitionState(new ConnectionDisconnectedState(Context, message.Error, Logger));
                    return TaskConstants.BooleanTrue;
                case ProtocolMessage.MessageAction.Error:
                    TransitionState(new ConnectionFailedState(Context, message.Error, Logger));
                    return TaskConstants.BooleanTrue;
            }

            return TaskConstants.BooleanFalse;
        }

        public override void AbortTimer()
        {
            _timer.Abort();
        }

        public override Task OnAttachToContext()
        {
            if (_inConnectTransition)
            {
                return TaskConstants.BooleanTrue;
            }

            var transport = Context.Transport;
            if (transport?.State == TransportState.Connected)
            {
                Context.SendToTransport(new ProtocolMessage(ProtocolMessage.MessageAction.Close));
                _timer.Start(TimeSpan.FromMilliseconds(CloseTimeout), OnTimeOut);
            }
            else
            {
                Context.SetState(new ConnectionClosedState(Context, Logger));
            }

            return TaskConstants.BooleanTrue;
        }

        private void OnTimeOut()
        {
            Context.Execute(() => Context.SetState(new ConnectionClosedState(Context, Logger)));
        }

        private void TransitionState(ConnectionStateBase newState)
        {
            _timer.Abort();
            Context.SetState(newState);
        }

        public override void Connect()
        {
            _inConnectTransition = true;
            _timer.Abort();
            Context.Connection.Key = string.Empty;
            Context.SetState(new ConnectionConnectingState(Context, Logger));
        }
    }
}
