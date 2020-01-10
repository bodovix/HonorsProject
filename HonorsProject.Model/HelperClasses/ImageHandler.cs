using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HonorsProject.Model.HelperClasses
{
    public class ImageHandler
    {
        public string OutputMessage { get; set; }
        public string host { get; set; }
        public string username { get; set; }
        public int port { get; set; }
        public string password { get; set; }

        public ImageHandler()
        {
            host = "mayar.abertay.ac.uk";
            username = "1701267";
            port = 22;
            password = "123Haggis0nToast123$";
        }

        private BitmapImage DownloadImageFromSFTP(string imageLocation)
        {
            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    using (MemoryStream fileStream = new MemoryStream())
                    {
                        client.DownloadFile(client.WorkingDirectory + "/" + imageLocation, fileStream);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = fileStream;
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }
                else
                {
                    OutputMessage = "I couldn't connect";
                    return null;
                }
            }
        }

        private bool SaveImageToFTPServer(ImageSource imageSource, string imageLocationMemory, string imageLocationDisk)
        {
            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, imageSource); // Function returns byte[] csv file

            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    using (var ms = new MemoryStream(biteArray))
                    {
                        client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                        client.UploadFile(ms, imageLocationDisk);// imageLocationDisk == openFileDialog.FileName
                        client.RenameFile(client.WorkingDirectory + "/" + imageLocationDisk, client.WorkingDirectory + "/" + imageLocationMemory);
                        return true;
                    }
                }
                else
                {
                    OutputMessage = "I couldn't connect";
                    return false;
                }
            }
        }

        private byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
        {
            byte[] bytes = null;
            var bitmapSource = imageSource as BitmapSource;

            if (bitmapSource != null)
            {
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (var stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    bytes = stream.ToArray();
                }
            }

            return bytes;
        }

        private bool DeleteImageFromFTPServer(string imageLocation)
        {
            //BitmapEncoder encoder = new TiffBitmapEncoder();
            // byte[] biteArray = ImageSourceToBytes(encoder, QuestionImage);

            using (var client = new Renci.SshNet.SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory("public_html/honors/questions");
                    //using (var ms = new MemoryStream(biteArray))
                    // {
                    //client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                    client.DeleteFile(client.WorkingDirectory + "/" + imageLocation);
                    // }
                    return true;
                }
                else
                {
                    OutputMessage = "Couldn't connect to SFTP Server";
                    return false;
                }
            }
        }
    }
}