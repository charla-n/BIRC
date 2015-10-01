using BIRC.Shared.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Streams;

namespace BIRC.Shared.Files
{
    public abstract class AbstractFiles<T> where T : class
    {
        private StorageFolder roaming = ApplicationData.Current.RoamingFolder;
        private StorageFolder appRoaming;
        protected string filename;

        public AbstractFiles(string filename)
        {
            this.filename = filename;
        }

        private async Task OpenRoamingFolder()
        {
            appRoaming = await roaming.CreateFolderAsync(App.APPNAME, CreationCollisionOption.OpenIfExists);
        }

        public abstract Task<T> ReadImpl();
        public abstract void WriteImpl(T obj);

        protected async Task<T> Read()
        {
            StorageFile file = null;
            string text = null;

            try
            {
                await OpenRoamingFolder();
                file = await appRoaming.GetFileAsync(filename);
                text = await FileIO.ReadTextAsync(file, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                return JsonConvert.DeserializeObject<T>(text);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                    return null;
                else if (e is UnauthorizedAccessException)
                    throw new ErrorBIRC(string.Format(new ResourceLoader().GetString("FileUnauthorized"), filename));
                else if (e is JsonReaderException || e is JsonSerializationException)
                {
                    await file.DeleteAsync();
                    return null;
                }
                throw;
            }
        }

        protected async void Write(T obj, uint ntry = 0)
        {
            StorageFile file = null;

            if (ntry > 1)
                throw new ErrorBIRC(string.Format(new ResourceLoader().GetString("FileWriteMaxTry"), filename));
            try
            {
                await OpenRoamingFolder();
                file = await appRoaming.GetFileAsync(filename);
                string json = JsonConvert.SerializeObject(obj);
                await FileIO.WriteTextAsync(file, json, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    file = await appRoaming.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                    Write(obj, ntry + 1);
                    return;
                }
                else if (e is UnauthorizedAccessException)
                    throw new ErrorBIRC(string.Format(new ResourceLoader().GetString("FileUnauthorized"), filename));
                throw;
            }

        }
    }
}
