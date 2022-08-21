namespace CourseProject.Services.Interfaces
{
    public interface IUnitOfWork
    {
        public ILikeRepository LikeRepository { get; }
        public ICommentRepository CommentRepository { get; }
        public ITagRepository TagRepository { get; }

        public IItemRepository ItemRepository { get; }

        public ICollectionRepository CollectionRepository { get; }
        public ICollectionPropertyRepository CollectionPropertyRepository { get; }
        public IPropertyRepository PropertyRepository { get; }

        public void Save();
    }
}
