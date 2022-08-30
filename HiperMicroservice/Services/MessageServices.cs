using HiperShared.Model;
using Newtonsoft.Json;
using System.Text;

namespace HiperMicroservice.Services
{
    public class MessageServices
    {
        public static MessageInputModel StringfyMessage(byte[]? bytes)
        {
            var contentString = Encoding.UTF8.GetString(bytes);
            var message = JsonConvert.DeserializeObject<MessageInputModel>(contentString);
            return message;
        }
        public static byte[] BytefyMessage(MessageInputModel message)
        {
            var stringfiedMessage = JsonConvert.SerializeObject(message);
            var bytesMessage = Encoding.UTF8.GetBytes(stringfiedMessage);
            return bytesMessage;
        }

        public static void NotifyUser(MessageInputModel message)
        {
            // use notification service ServiceProvider... maybe
        }

    }
}
