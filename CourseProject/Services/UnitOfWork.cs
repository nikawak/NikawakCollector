using CourseProject.Services.Interfaces;
using CourseProject.Services.Repositories;

namespace CourseProject.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbContextAsync _context;

        public UnitOfWork(DbContextAsync context)
        {
            _context = context;
        }
        private ILikeRepository _likeRepository;
        private ICommentRepository _commentRepository;
        private ITagRepository _tagRepository;
        private IItemRepository _itemRepository;
        private ICollectionRepository _collectionRepository;
        private ICollectionPropertyRepository _collectionPropertyRepository;
        private IPropertyRepository _propertyRepository;


        public ILikeRepository LikeRepository => _likeRepository ?? new LikeRepository(_context);
        public ICommentRepository CommentRepository => _commentRepository ?? new CommentRepository(_context);
        public ITagRepository TagRepository => _tagRepository ?? new TagRepository(_context);
        public IItemRepository ItemRepository => _itemRepository ?? new ItemRepository(_context);
        public ICollectionRepository CollectionRepository => _collectionRepository ?? new CollectionRepository(_context);
        public ICollectionPropertyRepository CollectionPropertyRepository => _collectionPropertyRepository ?? new CollectionPropertyRepository(_context);
        public IPropertyRepository PropertyRepository => _propertyRepository ?? new PropertyRepository(_context);

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
