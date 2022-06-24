using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Media;
using Windows.Media.Control;
using Windows.Media.Playback;
using Windows.Storage.Streams;

namespace GSMC {
    public class PlayerInfo {
        GlobalSystemMediaTransportControlsSessionMediaProperties Info { get; }
        GlobalSystemMediaTransportControlsSessionTimelineProperties Timeline { get; }
        GlobalSystemMediaTransportControlsSessionPlaybackInfo Playback { get; }

        public PlayerInfo(
            GlobalSystemMediaTransportControlsSessionMediaProperties properties,
            GlobalSystemMediaTransportControlsSessionTimelineProperties timelineProperties,
            GlobalSystemMediaTransportControlsSessionPlaybackInfo playbackInfo
        ) {
            this.Info = properties;
            this.Timeline = timelineProperties;
            this.Playback = playbackInfo;
        }

        public GlobalSystemMediaTransportControlsSessionMediaProperties GetProps() { return this.Info; }

        public bool IsPlaying { get { return Playback.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing; } }
        public bool IsPaused { get { return Playback.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused; } }
        public bool IsStopped { get { return Playback.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped; } }

        public GlobalSystemMediaTransportControlsSessionPlaybackStatus PlaybackStatus { get { return Playback.PlaybackStatus; } }

        public string ArtistName { get { return Info.Artist; } }
        public string SongName { get { return Info.Title; } }
    }

    public class Session {
        readonly GlobalSystemMediaTransportControlsSession session;

        public Session(GlobalSystemMediaTransportControlsSession session) {
            this.session = session;
        }

        public async Task<PlayerInfo> GetInfo() {
            return new PlayerInfo(
                await this.GetProps(),
                this.GetTimeline(),
                this.GetPlaybackInfo()
            );
        }

        public async Task<GlobalSystemMediaTransportControlsSessionMediaProperties> GetProps() {
            return await session.TryGetMediaPropertiesAsync();
        }

        public GlobalSystemMediaTransportControlsSessionTimelineProperties GetTimeline() {
            return session.GetTimelineProperties();
        }

        public GlobalSystemMediaTransportControlsSessionPlaybackInfo GetPlaybackInfo() {
            return session.GetPlaybackInfo();
        }

        public async Task Pause() {
            await session.TryPauseAsync();
        }

        public async Task Play() {
            await session.TryPlayAsync();
        }

        public async Task Next() {
            await session.TrySkipNextAsync();
        }

        public async Task Prev() {
            await session.TrySkipPreviousAsync();
        }
    }

    public class GSMTW {
        public GlobalSystemMediaTransportControlsSessionManager gsmt;

        public GSMTW() {
            this.gsmt = GlobalSystemMediaTransportControlsSessionManager.RequestAsync().GetAwaiter().GetResult();
        }

        public Session CurrentSession() {
            return new Session(gsmt.GetCurrentSession());
        }

        public List<Session> GetSession() {
            List<Session> ret = new List<Session>();
            foreach (var session in gsmt.GetSessions()) {
                ret.Add(new Session(session));
            }
            return ret;
        }
    }
}
