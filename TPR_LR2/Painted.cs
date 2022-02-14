using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TPR_LR2
{
    public class Painted
    {
        public int X;
        public int Y;
        public bool selected = false;
        public bool final = false;
        public bool isObj = false;
        public bool isCon = false;
        public bool isDis = false;
        public bool isTmp = false;
        public List<Painted> heirs;
        public Painted parent = null;
        public Color col;
        public string txt;
        public Rectangle rect;
        public double prob;

        public Painted(int counter, string whoIs, int Xo, int Yo)
        {
            this.X = Xo;
            this.Y = Yo;
            this.parent = null;
            this.heirs = new List<Painted>();
            this.final = false;
            this.prob = 0.0;
            if (whoIs == "obj")
            {
                var tmpObj = CreateTmp();
                Parenting(tmpObj, this);
            }

            if (whoIs == "obj")
            {
                isObj = true;
                col = Color.Cyan;
                txt = "x" + counter.ToString();
                rect = new Rectangle(X, Y, 125, 40);
            }
            else if (whoIs == "con")
            {
                isCon = true;
                col = Color.Magenta;
                txt = "||";
                rect = new Rectangle(X, Y, 40, 40);
            }
            else if (whoIs == "tmp")
            {
                isTmp = true;
                col = Color.Yellow;
                txt = "Add something";
                rect = new Rectangle(X, Y, 125, 40);
            }
            else
            {
                isDis = true;
                col = Color.Coral;
                txt = "&&";
                rect = new Rectangle(X, Y, 40, 40);
            }
        }

        public Painted Clone()
        {
            Painted newP = new Painted(0,"obj", this.X,this.Y);
            newP.isCon = this.isCon;
            newP.isObj = this.isObj;
            newP.isDis = this.isDis;
            newP.isTmp = this.isTmp;
            newP.selected = this.selected;
            newP.heirs = this.heirs;
            newP.col = this.col;
            newP.txt = this.txt;
            newP.rect = this.rect;
            newP.final = this.final;
            if (this.parent != null)
            {
                var a = this.parent;
                
                this.parent.heirs.Remove(this);
                this.parent.heirs.Add(newP);
                newP.parent = this.parent;
                this.parent = null;
            }
            for (int i =0; i < heirs.Count; ++i)
            {
                heirs[i].parent = newP;
            }
            return newP;
        }

        public void ChangeCoords(int X0, int Y0)
        {
            this.X += X0;
            this.Y += Y0;
            this.rect = new Rectangle(X, Y, 125, 40);
        }

        public void Parenting(Painted heir, Painted parent)
        {
            parent.heirs.Add(heir);
            heir.parent = parent;
        }

        public Painted CreateTmp()
        {
            Painted newP;
            if (this.isCon == true || this.isDis == true)
            {
                newP = new Painted(0, "tmp", this.X+40, this.Y);
                
            }
            else
                newP = new Painted(0, "tmp", this.X + 150, this.Y);
            return newP;
        }

    }
}
