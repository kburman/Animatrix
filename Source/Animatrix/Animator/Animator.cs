using Animatrix.Animation;
using Animatrix.Projectors;
using Animatrix.Screener;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Animatrix.Animator
{
    public class Animator :Component,IDisposable 
    {

        private System.Windows.Forms.Timer timer1;
        private IContainer components;
        private List<IProjector> lst;

        public Animator()
        {
            InitializeComponent();
            lst = new List<IProjector>();
            
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

        }

        public bool Started
            { get { return timer1.Enabled; } }

        public void Start()
        {
            this.timer1.Start();
        }

        public void Pause()
        {
            this.timer1.Stop();
        }

        private int _fps = 100;
        public int FPS
        {
            get
            {
                return _fps;
            }
            set
            {
                timer1.Interval = 1000 / value;
                _fps = value;
            }
        }


        private bool Exits(Control host)
        {
           
            bool exits = false;
            if (host == null) return false;
            if (host.IsDisposed) return false;
            Object iHost;
            lock (lst)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    iHost = lst[i].getHost();
                    if (iHost != null)
                        if (iHost.Equals(host))
                        {
                            exits = true;
                            break;
                        }

                }
            }
            iHost = null;

            return exits;
        }

        public void RemoveAll()
        {
            lock (lst)
            {
                for (int i = 0; i < lst.Count; i++)
                {
                    lst[i].leaveScreen();
                    lst[i].CleanMemoryFootprints();
                }
                lst.Clear();
            }
        }

        public void Add(IProjector pj)
        {
            lock (lst )
            {
                lst.Add(pj);
            }
        }

        public ManualResetEvent Add(Control cntrl, IAnimation animation)
        {
            if (cntrl == null) return null;
            if (cntrl.IsDisposed) return null ;
            if (animation == null) return null ;
            if (Exits(cntrl)) return null;
            IScreener  cs = new ControlScreener  ( cntrl);
            IProjector  pj = new Projector( cs,  animation);
            this.Add( pj);
            return ((Projector)pj).Wait;
        }


        public ManualResetEvent AddReflection(Control cntrl)
        {
            if (cntrl == null) return null;
            if (cntrl.IsDisposed) return null;
            if (Exits(cntrl)) return null;
            IScreener cs = new Reflector (cntrl);
            IProjector pj = new Projector(cs, new Reflection(cntrl.Parent.BackColor));
            this.Add(pj);
            return ((Projector)pj).Wait;
        }

        public ManualResetEvent AddReflection(Control cntrl, Reflection reflection)
        {
            if (cntrl == null) return null;
            if (cntrl.IsDisposed) return null;
            if (Exits(cntrl)) return null;
            IScreener cs = new Reflector(cntrl);
            IProjector pj = new Projector(cs, reflection);
            this.Add(pj);
            return ((Projector)pj).Wait;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //timer1.Stop();
            if (lst != null)
            {
                lock (lst)
                {
                    IProjector pj;
                    for (int i = 0; i < lst.Count; i++)
                    {
                        try
                        {
                              pj = lst[i];
                        if (pj.Completed)
                        {
                            pj.leaveScreen();
                            pj.CleanMemoryFootprints();
                            lst.RemoveAt(i);
                            --i;
                        }
                        else
                        {
                            pj.NextFrame();
                        }
                        }
                        catch (Exception ex)
                        {
                            
                            
                        }
                    }
                    pj = null;
                }
                //timer1.Start();
            }
        }



        public void Dispose()
        {
            RemoveAll();
            timer1.Stop();
            timer1.Dispose();
        }
    }
}
