using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPR_LR2
{
    public partial class Form1 : Form
    {
        public List<Painted> objs;
        public Painted start;

        public Form1()
        {
            InitializeComponent();
            pb.Location = new Point(132, 97);

            objs = new List<Painted>();
            start = new Painted(0, "tmp", 10, 10);
            objs.Add(start);

            if (pb.Image == null)
            {
                Bitmap bmp = new Bitmap(pb.Width, pb.Height);
                using (Graphics g1 = Graphics.FromImage(bmp))
                {
                    g1.Clear(Color.White);
                }
                pb.Image = bmp;
            }


            label5.Text = objs.Count.ToString();
            repaint();


        }

        public void repaint()
        {
            Pen pen = new Pen(Color.Black);
            Graphics g = Graphics.FromImage(pb.Image);
            g.Clear(pb.BackColor);
            foreach (Painted a in objs)
            {
                if (a.selected == true)
                    pen.Color = Color.Red;
                if (a.isTmp == true || a.isObj == true)
                {
                    Brush br = new SolidBrush(a.col);
                    if (a.final)
                        br = new SolidBrush(Color.GreenYellow);
                    g.FillRectangle(br,a.rect);
                    g.DrawRectangle(pen, a.rect);
                    g.DrawString(a.txt, new Font("Arial", 10), Brushes.Black, new Point(a.rect.X + 20, a.rect.Y + 10));
                    if (a.final)
                    {
                        g.DrawString("P=" + a.prob.ToString(), new Font("Arial", 10), Brushes.Black, new Point(a.rect.X + 60, a.rect.Y + 10));
                    }
                }
                else
                {
                    Brush br = new SolidBrush(a.col);
                    g.FillEllipse(br, a.rect);
                    g.DrawEllipse(pen, a.rect);
                    g.DrawString(a.txt, new Font("Arial", 10), Brushes.Black, new Point(a.rect.X + 10, a.rect.Y + 10));
                }
                if (a.parent!=null)
                {
                    pen.Color = Color.Black;
                    g.DrawLine(pen, new Point(a.rect.Location.X, a.rect.Location.Y+20), new Point(a.parent.rect.Location.X+a.parent.rect.Width, a.parent.rect.Bottom-20));

                }

            }
            pb.Invalidate();
            pb.Select();
        }

        //public void repaint()
        //{
            
        //    Graphics g = Graphics.FromImage(pb.Image);
        //    g.Clear(pb.BackColor);

        //    Pen pen = new Pen(Color.Black);
        //    if (start.selected)
        //    {
        //        pen.Color = Color.Red;
        //    }
        //    Brush br = new SolidBrush(objs[0].col);
        //    g.FillRectangle(br, start.rect);
        //    g.DrawRectangle(pen, start.rect);
        //    g.DrawString(start.txt, new Font("Arial", 10), Brushes.Black, new Point(start.rect.X + 20, start.rect.Y + 10));
        //    if (start.heirs.Count != 0)
        //    {
        //        foreach (Painted a in start.heirs)
        //        {
        //            kekw(a);
        //        }
        //    }
        //    pb.Invalidate();
        //    pb.Select();
        //}

        //public void kekw(Painted a)
        //{
        //    Pen pen = new Pen(Color.Black);
        //    Graphics g = Graphics.FromImage(pb.Image);
        //    g.Clear(pb.BackColor);

        //    if (a.selected == true)
        //        pen.Color = Color.Red;
        //    if (a.isTmp == true || a.isObj == true)
        //    {
        //        Brush br = new SolidBrush(a.col);
        //        g.FillRectangle(br, a.rect);
        //        g.DrawRectangle(pen, a.rect);
        //        g.DrawString(a.txt, new Font("Arial", 10), Brushes.Black, new Point(a.rect.X + 20, a.rect.Y + 10));
        //    }
        //    else
        //    {
        //        Brush br = new SolidBrush(a.col);
        //        g.FillEllipse(br, a.rect);
        //        g.DrawEllipse(pen, a.rect);
        //        g.DrawString(a.txt, new Font("Arial", 10), Brushes.Black, new Point(a.rect.X + 10, a.rect.Y + 10));
        //    }
        //    if (a.parent != null)
        //    {
        //        pen.Color = Color.Black;
        //        g.DrawLine(pen, new Point(a.rect.Location.X, a.rect.Location.Y + 20), new Point(a.parent.rect.Location.X + a.parent.rect.Width, a.parent.rect.Bottom - 20));

        //    }
        //    if (a.heirs.Count != 0)
        //    {
        //        foreach (Painted b in a.heirs)
        //        {
        //            kekw(b);
        //        }
        //    }
        //}


        Rectangle b;
        
        private void button1_Click(object sender, EventArgs e)
        {

            Painted selected = null;
            int ind = -1;
            for (int i = 0; i < objs.Count; ++i)
            {
                if (objs[i].selected)
                {
                    selected = objs[i];
                    ind = i;
                    break;
                }
            }
            if (selected != null)
            {
                if (rbObject.Checked)
                {
                    
                    objs[ind].selected = false;
                    Painted new_obj = new Painted(cntobj(), "obj", objs[ind].X, objs[ind].Y);
                    
                    if (objs[ind].parent != null)
                    {
                        Painted tmp = objs[ind];
                        objs[ind].parent.heirs.Add(new_obj);
                        new_obj.parent = objs[ind].parent;
                        objs[ind].ChangeCoords(0, 60);
                        objs.Remove(tmp);
                        objs.Add(tmp);
                        new_obj.parent.heirs.Remove(tmp);
                        new_obj.parent.heirs.Add(tmp);
                        if (tmp.parent.heirs.Count > 1)
                            repos(tmp);
                    }
                    else
                    {
                        //new_obj.heirs.Add(objs[ind]);
                        //objs[ind].parent = new_obj;
                        //var a = objs[ind];
                        objs.Remove(objs[0]);
                        //objs.Add(a);
                    }


                    //Painted tmp = objs[ind].Clone();
                    objs.Add(new_obj);
                    if (cb1.Checked && tbProb!=null)
                    {
                        new_obj.final = true;
                        new_obj.prob = Convert.ToDouble(tbProb.Text);
                        new_obj.heirs[0] = null;
                        new_obj.heirs.Clear();
                    }
                    else 
                        objs.Add(new_obj.heirs[0]);
                    label5.Text = objs.Count.ToString();


                    // tmp.Parenting(tmp, objs[ind]);
                    //tmp.ChangeCoords(0, 60);
                    // objs.Add(tmp);
                    label2.Text += objs.Count.ToString();
                }
                else
                {
                    objs[ind].selected = false;
                    Painted new_obj;
                    if (rbDisjunction.Checked)
                        new_obj = new Painted(objs.Count, "dis", objs[ind].X, objs[ind].Y);
                    else
                        new_obj = new Painted(objs.Count, "con", objs[ind].X, objs[ind].Y);

                    Painted tmp = objs[ind];
                    objs[ind].parent.heirs.Add(new_obj);
                    new_obj.parent = objs[ind].parent;
                    objs[ind].ChangeCoords(60, 0);
                    objs[ind].parent = new_obj;
                    new_obj.heirs.Add(objs[ind]);
                    objs.Add(new_obj);
                    objs.Remove(tmp);
                    objs.Add(tmp);
                    
                    label5.Text = objs.Count.ToString();
                    //objs.Add(new_obj.heirs[0]);

                    //Painted tmp = objs[ind].Clone();
                    //new_obj.Parenting(new_obj, objs[ind]);
                    //objs[ind] = new_obj;
                    //objs.Add(new_obj.heirs[0]);
                    //tmp.Parenting(tmp, objs[ind]);
                    //tmp.ChangeCoords(50, 0);
                    //objs.Add(tmp);
                    label2.Text += objs.Count.ToString();
                }

            }





            repaint();
            pb.Select();

        }



        public void repos(Painted pain)
        {
            Painted nex = return_par(pain);
            int ind = -1;
            for (int i = 0; i < nex.parent.heirs.Count; ++i)
            {
                if (nex.parent.heirs[i].txt == nex.txt)
                {
                    ind = i;
                    label4.Text = ind.ToString();
                    break;
                }
            }
            if ((ind + 1) < nex.parent.heirs.Count)
            {
                for (int i = (ind + 1); i < nex.parent.heirs.Count; ++i)
                {
                    return_heir(nex.parent.heirs[i]);
                }
            }

        }

        public Painted return_par(Painted pain)
        {
            Painted tmp = pain;
            while (tmp.parent != objs[1])
            {
                tmp = tmp.parent;
            }
            return tmp;
        }

        public void return_heir(Painted pain)
        {
            if (pain.heirs.Any())
            {
                for (int i = 0; i < pain.heirs.Count; ++i)
                {
                    return_heir(pain.heirs[i]);
                }
                
            }
            pain.ChangeCoords(0, 60);
        }

        private void buttnResult_Click(object sender, EventArgs e)
        {
            if (tbPrice.Text!=null)
                calc();
        }

        string ans;
        string ans_f;
        string ans_p;
        double ans_c;
        public void calc()
        {
            ans = "(";
            ans_f = "(";
            ans_p = "(";
            ans_c = 1.0;
            tmp = 1.0;
            if (objs[1].isCon)
            {
                ans_f += "!";
                ans_p += "(1-";
            }
            for (int i = 0; i < objs[1].heirs.Count;++i)
            {
                if (!objs[1].heirs[i].isTmp)
                {
                    smh(objs[1].heirs[i]);
                    if (objs[1].isCon)
                        tmp = (1.0 - tmp);
                    ans_c *= tmp;
                    tmp = 1.0;
                }
                if (i < objs[1].heirs.Count - 2)
                {
                    ans += objs[1].txt + " ";
                    //if (objs[1].isCon)
                        ans_f += "&&" + " ";
                    ans_p += "*" + " ";
                    //else
                    //ans_f += "&&" + " ";
                }
                
            }
            ans += ")";
            ans_f += ")";
            ans_p += ")";
            
            label6.Text = "ФАЛ f = " + ans;
            label7.Text = "ФАЛ f'= " + ans_f;
            label8.Text = "ФППЗ P = " + ans_p;
            double pr = Convert.ToInt32(tbPrice.Text) * ans_c;
            label9.Text = "Вероятность опасного состояния = " + ans_c.ToString() + " Price of error = " + pr.ToString();
        }
        double tmp;

        public void smh(Painted pain)
        {
            //tmp = 1.0;
            if (pain.heirs.Any())
            {
                ans += "(";
                if (pain.isCon)
                {
                    ans_f += "!";
                    ans_p += "(1-";
                }
                ans_f += "(";
                ans_p += "(";
                for (int i = 0; i < pain.heirs.Count; ++i)
                {
                    smh(pain.heirs[i]);

                    if (pain.isCon || pain.isDis)
                    {
                        
                        if (i < pain.heirs.Count - 2)
                        {
                            ans += pain.txt + " ";
                            //if (pain.isCon)
                                ans_f += "&&" + " ";
                            ans_p += "*" + " ";
                            
                            //else
                            //ans_f += "&&" + " ";

                        }
                        
                       // else
                            //ans_c *= tmp;
                    }
                            
                }
                ans += ")";
                ans_f += ")";
                ans_p += ")";
                if (pain.isCon)
                    tmp = (1.0 - tmp);
                //ans_c *= tmp;


            }
            if (pain.final)
            {
                ans += pain.txt + " ";
                if (pain.parent.isCon)
                {
                    ans_f += "!";
                    ans_p += "(1-";
                    tmp *= (1.0 - pain.prob);
                }
                else
                    tmp *= pain.prob;
                ans_f += pain.txt + " ";
                ans_p += pain.prob.ToString() + ") ";
                
            }
        }


        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = pb.PointToClient(System.Windows.Forms.Cursor.Position);
            label1.Text = "X=" + (p.X).ToString() + " Y=" + (p.Y).ToString();
        }

        private void pb_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = pb.PointToClient(System.Windows.Forms.Cursor.Position);
            //select(p,start);








            for (int i = 0; i < objs.Count; ++i)
            {
                if (objs[i].isCon || objs[i].isDis)
                {
                    if (inCircle(objs[i], p))
                    {
                        for (int j = 0; j < objs.Count; ++j)
                        {
                            if (j != i)
                                objs[j].selected = false;
                        }
                        objs[i].selected = !objs[i].selected;
                        label2.Text = i.ToString();
                        if (objs[i].parent != null)
                            label3.Text = objs[i].parent.X.ToString();
                        repaint();
                        return;
                    }
                }
                if (objs[i].rect.Contains(p))
                {
                    for (int j = 0; j < objs.Count; ++j)
                    {
                        if (j != i)
                            objs[j].selected = false;
                    }
                    objs[i].selected = !objs[i].selected;
                    label2.Text = i.ToString();
                    if (objs[i].parent != null)
                        label3.Text = objs[i].parent.X.ToString();
                    repaint();
                    return;
                }
            }

            //Pen pen = new Pen(Color.Black);
            //Graphics g = Graphics.FromImage(pb.Image);
            //g.Clear(pb.BackColor);
            //g.DrawEllipse(pen, p.X-10, p.Y-10, 40, 40);
            //pb.Invalidate();

            repaint();


        }

        public void select(Point p, Painted k)
        {
            while (k.heirs.Count != 1)
            {
                foreach (Painted pain in k.heirs)
                {
                    if (pain.isCon || pain.isDis)
                    {
                        if (inCircle(pain, p))
                        {
                            //for (int j = 0; j < objs.Count; ++j)
                            //{
                            //    if (j != i)
                            //        objs[j].selected = false;
                            //}
                            pain.selected = !pain.selected;
                            label2.Text = pain.X.ToString();
                            if (pain.parent != null)
                                label3.Text = pain.parent.X.ToString();
                            repaint();
                            return;
                        }
                    }
                    else if (pain.rect.Contains(p))
                    {
                        //for (int j = 0; j < objs.Count; ++j)
                        //{
                        //    if (j != i)
                        //        objs[j].selected = false;
                        //}
                        pain.selected = !pain.selected;
                        label2.Text = pain.X.ToString();
                        if (pain.parent != null)
                            label3.Text = pain.parent.X.ToString();
                        repaint();
                        return;
                    }
                    else
                    {

                    }
                }
            
            }
        }

        public int cntobj()
        {
            int count = 0;
            foreach (Painted a in objs)
            {
                if (a.isObj)
                    count++;
            }
            return count;
        }

        public bool inCircle(Painted pain, Point p)
        {
            Point center = new Point(
                  pain.X - 20,
                  pain.Y - 20);

            double _xRadius = 20;
            double _yRadius = 20;


            if (_xRadius <= 0.0 || _yRadius <= 0.0)
                return false;
            /* This is a more general form of the circle equation
             *
             * X^2/a^2 + Y^2/b^2 <= 1
             */

            Point normalized = new Point(p.X - center.X,
                                         p.Y - center.Y);

            return ((double)(normalized.X * normalized.X)
                     / (_xRadius * _xRadius)) + ((double)(normalized.Y * normalized.Y) / (_yRadius * _yRadius))
                <= 1.0;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void cb1_CheckedChanged(object sender, EventArgs e)
        {
            if (cb1.Checked)
            {
                rbObject.Checked = true;
            }
        }
    }
}
