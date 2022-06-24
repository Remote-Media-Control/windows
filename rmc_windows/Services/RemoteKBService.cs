using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GSMC;
using Microsoft.Extensions.Logging;
using Windows.Media.Control;

namespace rmc_windows {
    public class RemotePlayerService : RemotePlayer.RemotePlayerBase {
        private readonly ILogger<RemotePlayerService> _logger;
        protected GSMTW gsmtw;

        public RemotePlayerService(ILogger<RemotePlayerService> logger) {
            _logger = logger;
            gsmtw = new GSMTW();
        }

        public override async Task<Empty> Run(RunRequest request, ServerCallContext context) {
            var session = gsmtw.CurrentSession();
            switch (request.Key) {
                case Key.Next:
                    await session.Next();
                    break;
                case Key.Prev:
                    await session.Prev();
                    break;
                case Key.Pause:
                    await session.Pause();
                    break;
                case Key.Play:
                    await session.Play();
                    break;
                case Key.None:
                default:
                    break;
            }
            return new Empty { };
        }

        public override async Task<InfoResponse> Info(Empty request, ServerCallContext context) {
            var session = gsmtw.CurrentSession();
            var info = await session.GetInfo();
            return new InfoResponse {
                SongName = info.SongName,
                ArtistName = info.ArtistName,
                Status = info.PlaybackStatus switch {
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing => PlayerStatus.Playing,
                    GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused => PlayerStatus.Paused,
                    _ => PlayerStatus.Unknown,
                }
            };
        }
    }
}
