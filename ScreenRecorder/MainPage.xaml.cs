using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ScreenRecorder
{    
    public sealed partial class MainPage : Page
    {
        private MediaCapture mCap;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override async void OnNavigatedFrom(NavigationEventArgs e)
        {
            await StopRecording();
        }

        private async Task StopRecording()
        {
            // Stop recording and dispose resources
            if (mCap != null)
            {
                await mCap.StopRecordAsync();
                mCap.Dispose();
                mCap = null;
            }
        }

        //Create File for Recording on Phone/Storage
        private static async Task<StorageFile> GetScreenRecVdo(CreationCollisionOption creationCollisionOption = CreationCollisionOption.ReplaceExisting)
        {
            return await KnownFolders.SavedPictures.CreateFileAsync("VehicleDetails.mp4", creationCollisionOption);
        }

        //Record the Screen
        private async void btnRecord_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (btnRecord.IsChecked.HasValue && btnRecord.IsChecked.Value)
            {
                // Initialization - Set the current screen as input
                var scrCaptre = ScreenCapture.GetForCurrentView();              

                mCap = new MediaCapture();
                await mCap.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    VideoSource = scrCaptre.VideoSource,
                    AudioSource = scrCaptre.AudioSource,
                });

                // Start Recording to a File and set the Video Encoding Quality
                var file = await GetScreenRecVdo();
                await mCap.StartRecordToStorageFileAsync(MediaEncodingProfile.CreateMp4(VideoEncodingQuality.Auto), file);
            }
            else
            {
                // Stop recording and start playback of the file
                await StopRecording();

                //If Media Element is taken on XAML
                //var file = await GetScreenRecVdo(CreationCollisionOption.OpenIfExists);
                //OutPutScreen.SetSource(await file.OpenReadAsync(), file.ContentType);
            }
        }       
    }
}
