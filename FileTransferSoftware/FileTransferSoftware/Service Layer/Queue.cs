using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTransferSoftware.Service_Layer
{
    public class QueueItems
    {
        public string filePath {  get; set; }
        public string transferStatus { get; set; }

       public QueueItems() 
        {
            transferStatus = "Pending";
        }
    }

    public class Queue
    {

        List<QueueItems> fileQueue = new List<QueueItems>();


        public void enqueue(string path,string status)
        {
            QueueItems item = new QueueItems();
            item.filePath = path;
            item.transferStatus = status;
            fileQueue.Add(item);
        }

        public QueueItems dequeue()
        {
            if (fileQueue.Count > 0)
            {
                QueueItems removedItem = fileQueue[0];
                fileQueue.RemoveAt(0);
                return removedItem;
            }
            else { return null; }
        }

        public bool isEmpty()
        {
            return fileQueue.Count == 0 ;
        }

        public List<QueueItems> getQueue()
        {
            return fileQueue;
        }
        public void clearQueue()
        {
            fileQueue.Clear();
        }

        public void removeQueueItemByPath(string path)
        {
            if (!isEmpty())
            {
                var itemToRemove = fileQueue.FirstOrDefault(item => item.filePath == path);
                fileQueue.Remove(itemToRemove);
            }
        }
    }
}
