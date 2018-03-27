using Discord;
using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord.Audio;
using motBot2.Services.YouTube;

namespace motBot2.Services
{
    public class SongService
    {
        private IVoiceChannel _voiceChannel;
        private BufferBlock<IPlayable> _songQueue;
        private IAudioClient audioClient;

        private readonly ConcurrentDictionary<ulong, IAudioClient> ConnectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();
        public Services.AudioPlaybackService AudioPlaybackService = new Services.AudioPlaybackService(); //{ get; set; }
        public IPlayable NowPlaying { get; private set; }

        public SongService()
        {
            _songQueue = new BufferBlock<IPlayable>();
        }
        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            IAudioClient client;
            if (ConnectedChannels.TryGetValue(guild.Id, out client))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            audioClient = await target.ConnectAsync();

            if (ConnectedChannels.TryAdd(guild.Id, audioClient))
            {
                // add log service
                Console.WriteLine("Connected to channel " + target);
            }
            this._voiceChannel = target;
            ProcessQueue();
        }
        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Console.WriteLine("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    if (_voiceChannel == null)
                    {
                        Console.WriteLine("Not connected to a voice channel");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Connected to channel " + Convert.ToString(_voiceChannel));
                    }

                    await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while playing song: {e}");
                }
            }
        }
        public void Queue(IPlayable video)
        {
            _songQueue.Post(video);
        }
        public IList<IPlayable> Clear()
        {
            _songQueue.TryReceiveAll(out var skippedSongs);

            Console.WriteLine($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }
        public void Next()
        {
            AudioPlaybackService.StopCurrentOperation();
        }
    }
}
