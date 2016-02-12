namespace CourseValidator
{
    public class Course
    {
        public readonly TableOfContents Toc;
        public readonly string ProjectRoot;

        public Course(string TocFile)
        {
            Toc = new TableOfContents(TocFile);
        }
    }
}
