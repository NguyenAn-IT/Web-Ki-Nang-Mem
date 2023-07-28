using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Web.Models
{
    public partial class Soft : DbContext
    {
        public Soft()
            : base("name=Soft1")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Course)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);
        }

        public void DeleteCourse(int id)
        {
            Course course = Courses.Find(id);
            Courses.Remove(course);
            SaveChanges();
        }

        // Thêm hàm sửa khóa học
        public void EditCourse(Course course)
        {
            Entry(course).State = EntityState.Modified;
            SaveChanges();
        }

        public Course GetCourseById(int id)
        {
            using (var context = new Soft())
            {
                return context.Courses.SingleOrDefault(c => c.Id == id);
            }
        }

        internal object FirstOrDefault(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}
