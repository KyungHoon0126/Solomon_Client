using Newtonsoft.Json;
using Prism.Mvvm;
using Solomon.Core.Bulletin.Model;
using System.Collections.Generic;

namespace Solomon.Core.Bulletin.Service.Response
{
    public class GetCommentListResponse : BindableBase
    {
        private List<CommentModel> _comments;
        [JsonProperty("comments")]
        public List<CommentModel> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);
        }
    }
}
