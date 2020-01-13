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
        public string Host { get; set; }
        public string Username { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string SFTPWorkingDirectory { get; set; }

        public ImageHandler(string ftpPathToSaveTo)
        {
            Host = "mayar.abertay.ac.uk";
            Username = "1701267";
            Port = 22;
            Password = "123Haggis0nToast123$";
            SFTPWorkingDirectory = ftpPathToSaveTo; // e.g. "public_html/honors/questions"
        }

        public BitmapImage DownloadImageFromSFTP(BitmapImage bitmapImage, string imageLocation)
        {
            using (var client = new Renci.SshNet.SftpClient(Host, Port, Username, Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory(SFTPWorkingDirectory);
                    using (MemoryStream fileStream = new MemoryStream())
                    {
                        client.DownloadFile(client.WorkingDirectory + "/" + imageLocation, fileStream);
                        bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = fileStream;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }
                else
                {
                    OutputMessage = "Couldn't connect to SFTP server.";
                    return null;
                }
            }
        }

        public bool SaveImageToFTPServer(ImageSource imageSource, string imageLocationMemory, string imageLocationDisk)
        {
            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, imageSource); // Function returns byte[] csv file

            using (var client = new Renci.SshNet.SftpClient(Host, Port, Username, Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory(SFTPWorkingDirectory);
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
                    OutputMessage = "Couldn't connect to SFTP server.";
                    return false;
                }
            }
        }

        public bool WriteImageSourceAsByteArraySFTP(ImageSource imageSource, string imageLocationMemory)
        {
            BitmapEncoder encoder = new TiffBitmapEncoder();
            byte[] biteArray = ImageSourceToBytes(encoder, imageSource); // Function returns byte[] csv file

            using (var client = new Renci.SshNet.SftpClient(Host, Port, Username, Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory(SFTPWorkingDirectory);
                    using (var ms = new MemoryStream(biteArray))
                    {
                        client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                        client.Create(SFTPWorkingDirectory + "/" + imageLocationMemory);
                        client.WriteAllBytes(SFTPWorkingDirectory + "/" + imageLocationMemory, biteArray);// imageLocationDisk == openFileDialog.FileName
                        return true;
                    }
                }
                else
                {
                    OutputMessage = "Couldn't connect to SFTP server.";
                    return false;
                }
            }
        }

        public bool ReadByteArrayFromSFTP(ref byte[] bytesLocal, string imageLocationMemory)
        {
            byte[] bytesFTP;

            using (var client = new Renci.SshNet.SftpClient(Host, Port, Username, Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory(SFTPWorkingDirectory);
                    bytesFTP = client.ReadAllBytes(client.WorkingDirectory + "/" + imageLocationMemory);// imageLocationDisk == openFileDialog.FileName
                    if (bytesFTP != null)
                    {
                        bytesLocal = bytesFTP.ToArray();
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    OutputMessage = "Couldn't connect to SFTP server.";
                    return false;
                }
            }
        }

        public byte[] ImageSourceToBytes(BitmapEncoder encoder, ImageSource imageSource)
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

        public ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage bitmapImage = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.EndInit();

            ImageSource imgSrc = bitmapImage as ImageSource;

            return imgSrc;
        }

        public bool DeleteFileFromFTPServer(string imageLocation)
        {
            //BitmapEncoder encoder = new TiffBitmapEncoder();
            // byte[] biteArray = ImageSourceToBytes(encoder, QuestionImage);

            using (var client = new Renci.SshNet.SftpClient(Host, Port, Username, Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    client.ChangeDirectory(SFTPWorkingDirectory);
                    //using (var ms = new MemoryStream(biteArray))
                    // {
                    //client.BufferSize = (uint)ms.Length; // bypass Payload error large files
                    client.DeleteFile(client.WorkingDirectory + "/" + imageLocation);
                    // }
                    return true;
                }
                else
                {
                    OutputMessage = "Couldn't connect to SFTP server";
                    return false;
                }
            }
        }
    }
}