namespace CourseValidator
{
    public class Course
    {
        public readonly string ProjectRoot;
        public readonly TableOfContents Toc;

        public Course(string projectRoot, string tocFile)
        {
            ProjectRoot = projectRoot;
            Toc = new TableOfContents(tocFile, this);
        }
    }
}
