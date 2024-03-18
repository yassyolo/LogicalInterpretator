namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private TreeNode root;
        public Form1()
        {
            InitializeComponent();
            // Build the tree (you should have the logic for this)
            string expression = "Your expression here";
            root = BuildTree(expression);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            // Draw the tree
            DrawTree(root, e.Graphics, ClientRectangle.Width / 2, 50, 200);
        }

        private void DrawTree(TreeNode node, Graphics g, int x, int y, int xOffset)
        {
            if (node == null)
                return;

            // Draw the current node
            g.FillEllipse(Brushes.LightBlue, x - 20, y - 20, 40, 40);
            g.DrawEllipse(Pens.Black, x - 20, y - 20, 40, 40);
            g.DrawString(node.Name, Font, Brushes.Black, x - 10, y - 10);

            // Draw lines to children
            int childOffset = 50;

            if (node.OperandA != null)
            {
                int childX = x - xOffset;
                int childY = y + childOffset;
                g.DrawLine(Pens.Black, x, y, childX, childY);
                DrawTree(node.OperandA, g, childX, childY, xOffset / 2);
            }

            if (node.OperandB != null)
            {
                int childX = x + xOffset;
                int childY = y + childOffset;
                g.DrawLine(Pens.Black, x, y, childX, childY);
                DrawTree(node.OperandB, g, childX, childY, xOffset / 2);
            }
        }
    }
}
