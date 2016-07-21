using Microsoft.Win32;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlobDownloader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            txtAccountName.Text = "imagecapture";
            txtAccountKey.Text = "h4anynXx6HgTJU31e1rvfiEacDd/fGZur6s3Wvvkyd89Gt+0ed+Agyp/YX/DPJSJ5DkGHOpKwY/MrDbBb+sijA==";

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string azureStorageAccount = txtAccountName.Text.Trim();
                string azureStoragePrimaryKey = txtAccountKey.Text;
                string localfolderpath = txtFolderName.Text;

                if (txtFolderName.Text == "")
                {
                    MessageBox.Show("Please select a local folder to download");
                    return;
                }

                // Create an Azure CloudStorageAccount object.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + azureStorageAccount + ";AccountKey=" + azureStoragePrimaryKey);

                // Create a CloudBlobClient object using the CloudStorageAccount.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                lblStatusbarText.Text = "Starting download";

                // Loop through all the containers
                foreach (CloudBlobContainer container in blobClient.ListContainers(null))
                {
                    
                    System.IO.Directory.CreateDirectory(localfolderpath + "\\" + container.Name);
                    // Loop over items within the container 
                    foreach (IListBlobItem item in container.ListBlobs(null, false))
                    {
                        if (item.GetType() == typeof(CloudBlockBlob))
                        {
                            CloudBlockBlob blob = (CloudBlockBlob)item;
                            
                            blob.DownloadToFile(localfolderpath + "\\" + item.Container.Name + "\\" + blob.Name, System.IO.FileMode.Create, null, null);
                        }
                    }
                    lblStatusbarText.Text = container.Name + "is being downloaded";
                }
                lblStatusbarText.Text = "Done downloading";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}