using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace Solomon.Core.Bulletin.Model
{
    public class CommentModel : BindableBase, ICloneable
    {
        private int _commentIdx;
        [JsonProperty("comment_idx")]
        public int CommentIdx
        {
            get => _commentIdx;
            set
            {
                SetProperty(ref _commentIdx, value);
            }
        }

        private int _bulletinIdx;
        [JsonProperty("bulletin_idx")]
        public int BulletinIdx
        {
            get => _bulletinIdx;
            set
            {
                SetProperty(ref _bulletinIdx, value);
            }
        }

        private string _content;
        [JsonProperty("content")]
        public string Content
        {
            get => _content;
            set
            {
                SetProperty(ref _content, value);
            }
        }

        private string _writer;
        [JsonProperty("writer")]
        public string Writer
        {
            get => _writer;
            set
            {
                SetProperty(ref _writer, value);
            }
        }

        public object Clone()
        {
            return new CommentModel
            {
                CommentIdx = this.CommentIdx,
                BulletinIdx = this.BulletinIdx,
                Writer = this.Writer,
                Content = this.Content
            };
        }
    }
}
