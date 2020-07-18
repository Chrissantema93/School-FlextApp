using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FlextCamera
{
    public partial class MainPage : ContentPage
    {

        const string subscriptionKey = "fe233b1c64cc4844a5c6b16bb5dac3bd";
        const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/analyze";
        public int stoelNummer = 1;

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }


        private async void MakeAnalysisRequest(string imageFilePath, string filename, int stoelID)
        {
            try
            {
                HttpClient client = new HttpClient();

                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

                string requestParameters = "visualFeatures=Categories,Description";

                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;
                

                byte[] byteData = GetImageAsByteArray(imageFilePath);

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {

                    content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    response = await client.PostAsync(uri, content);
                }

                string stoelString = ",\"stoel\"" + ":\"" + stoelID + "\"}}";
                string contentString = await response.Content.ReadAsStringAsync();
                contentString = contentString.Substring(0, contentString.Length - 2);
                contentString += stoelString;

                      


                StringContent mycontentString = new StringContent(contentString, Encoding.UTF8, "application/json");
                string uri2 = "http://flextcamera.azurewebsites.net/api/Image";

                try
                {
                    using (var response2 = await client.PostAsync(uri2, mycontentString))
                    {
                        await DisplayAlert("Succes",response2.StatusCode.ToString(), "Okay");
                        //Console.WriteLine(response2.ToString());
                    }
                }
                catch(HttpRequestException e)
                {
                    await DisplayAlert("Error", e.ToString(), "Okay");
                    //Console.WriteLine(e.InnerException.Message);
                }
                
               
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);

            }
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                stoelNummer = selectedIndex + 1;
            }
        }


        public MainPage()
        {
            var picker = new Picker { Title = "Kies een flexplek" };
            picker.Items.Add("Flexplek 1");
            picker.Items.Add("Flexplek 2");
            picker.Items.Add("Flexplek 3");
            picker.Items.Add("Flexplek 4");
            picker.Items.Add("Flexplek 5");
            picker.Items.Add("Flexplek 6");
            picker.Items.Add("Flexplek 7");
            picker.Items.Add("Flexplek 8");

            InitializeComponent();


            takePhoto.Clicked += async (sender, args) =>
            {

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                   await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Test",
                    SaveToAlbum = true,
                    CompressionQuality = 75,
                    CustomPhotoSize = 50,
                    PhotoSize = PhotoSize.MaxWidthHeight,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });

                if (file == null)
                    return;

                var fileloc = file.Path;
                byte[] byteData = GetImageAsByteArray(fileloc);
                
                MakeAnalysisRequest(fileloc, file.ToString(), stoelNummer);

                //image.Source = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();

                //    file.Dispose();
                //    return stream;
                //});
            };

            


        }
    }
}
