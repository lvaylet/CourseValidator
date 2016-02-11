namespace CourseValidator
{
    public class Course
    {
        public readonly TableOfContents Toc;

        public Course(string TocFile)
        {
            Toc = new TableOfContents(TocFile);
        }
    }
}
