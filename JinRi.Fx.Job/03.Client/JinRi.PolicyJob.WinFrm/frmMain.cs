using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;

namespace JinRi.PolicyJob.WinFrm
{
    public partial class frmMain : Form
    {
        #region Field
        /// <summary>
        /// 调度器
        /// </summary>
        IScheduler scheduler;
        #endregion

        public frmMain()
        {
            InitializeComponent();

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = schedulerFactory.GetScheduler();
            
            int interval = 0;
            if (!int.TryParse(ConfigurationManager.AppSettings["RefreshTimes"], out interval) || interval < 5000)
            {
                interval = 5000;
            }
            timer1.Interval = interval;
            this.Load += frmMain_Load;
        }

        #region Event
        private void frmMain_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("名称", 100);
            listView1.Columns.Add("说明", 100);
            listView1.Columns.Add("状态", 90);
            listView1.Columns.Add("上次执行时间", 100);
            listView1.Columns.Add("下次执行时间", 100);
            listView1.Columns.Add("频率", 215);
            ImageList imgList = new ImageList();
            imgList.ImageSize = new Size(1, 28);
            listView1.SmallImageList = imgList;
            listView1.ShowGroups = true;
            listView1.HideSelection = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (scheduler.IsStarted)
            {
                scheduler.ResumeAll();
            }
            else
            {
                scheduler.Start();
            }
            BindData();
            timer1.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            scheduler.PauseAll();
            timer1.Stop();
            BindData();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                BindData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }   
        #endregion

        #region Method
        private void BindData()
        {
            listView1.Items.Clear();
            listView1.Groups.Clear();
            string expression, nextTime, previousTime;

            if (scheduler != null && !scheduler.IsShutdown)
            {
                IList<string> groupNames = scheduler.GetTriggerGroupNames();
                foreach (string groupName in groupNames)
                {
                    List<ITrigger> triggers = GetTriggers(GroupMatcher<TriggerKey>.GroupEquals(groupName));
                    ListViewGroup group = new ListViewGroup(groupName);
                    foreach (ITrigger trigger in triggers)
                    {
                        expression = GetTriggerExpression(trigger);
                        nextTime = trigger.GetNextFireTimeUtc().HasValue ? trigger.GetNextFireTimeUtc().Value.LocalDateTime.ToString("HH:mm:ss") : "";
                        previousTime = trigger.GetPreviousFireTimeUtc().HasValue ? trigger.GetPreviousFireTimeUtc().Value.LocalDateTime.ToString("HH:mm:ss") : "";
                        if (trigger.GetPreviousFireTimeUtc().HasValue && trigger.GetPreviousFireTimeUtc().Value.LocalDateTime.Date < DateTime.Now.Date)
                        {
                            previousTime = "- " + previousTime;
                        }
                        if (trigger.GetNextFireTimeUtc().HasValue && trigger.GetNextFireTimeUtc().Value.LocalDateTime.Date > DateTime.Now.Date)
                        {
                            nextTime = "+ " + nextTime;
                        }
                        TriggerState state = scheduler.GetTriggerState(trigger.Key);
                        ListViewItem item = new ListViewItem(
                                                                new string[]{
                                                                trigger.JobKey.Name,
                                                                scheduler.GetJobDetail(trigger.JobKey).Description,
                                                                state.ToString(),
                                                                previousTime,
                                                                nextTime,
                                                                expression
                                                        });
                        item.Group = group;
                        item.Tag = trigger;
                        item.BackColor = state == TriggerState.Normal ? Color.Transparent : Color.FromArgb(255, 204, 204);
                        listView1.Groups.Add(group);
                        listView1.Items.Add(item);
                    }
                }
            }
        }

        private string GetTriggerExpression(ITrigger trigger)
        {
            string expression = string.Empty;
            if (trigger is ISimpleTrigger)
            {
                ISimpleTrigger simple = (ISimpleTrigger)trigger;
                expression = "重复次数:" + (simple.RepeatCount == -1 ? "无限" : (simple.TimesTriggered.ToString() + "/" + simple.RepeatCount.ToString())) + ", 时间间隔:" + simple.RepeatInterval;
            }
            if (trigger is ICronTrigger)
            {
                ICronTrigger cron = (ICronTrigger)trigger;
                expression = cron.CronExpressionString;
            }
            return expression;
        }

        private List<IJobDetail> GetJobs(GroupMatcher<JobKey> matcher)
        {
            try
            {
                Quartz.Collection.ISet<JobKey> jobKeys = scheduler.GetJobKeys(matcher);

                List<IJobDetail> jobDetails = new List<IJobDetail>();
                foreach (JobKey key in jobKeys)
                {
                    jobDetails.Add(scheduler.GetJobDetail(key));
                }
                return jobDetails;
            }
            catch (SchedulerException e)
            {

            }
            return null;
        }

        private List<ITrigger> GetTriggers(GroupMatcher<TriggerKey> matcher)
        {
            try
            {
                Quartz.Collection.ISet<TriggerKey> Keys = scheduler.GetTriggerKeys(matcher);
                List<ITrigger> triggers = new List<ITrigger>();

                foreach (TriggerKey key in Keys)
                {
                    ITrigger trigger = scheduler.GetTrigger(key);
                    triggers.Add(trigger);
                }
                return triggers;
            }
            catch (Exception e)
            {

            }
            return null;
        }
        #endregion

        #region ContextMenuStrip Event
        private void StartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ITrigger trigger = listView1.SelectedItems[0].Tag as ITrigger;
                if (trigger != null)
                {
                    scheduler.ResumeTrigger(trigger.Key);
                    BindData();
                }
            }
        }

        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ITrigger trigger = listView1.SelectedItems[0].Tag as ITrigger;
                if (trigger != null)
                {
                    scheduler.PauseTrigger(trigger.Key);
                    BindData();
                }
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void StartAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (scheduler.IsStarted)
            {
                scheduler.ResumeAll();
            }
            else
            {
                scheduler.Start();
            }
            BindData();
            timer1.Start();
        }

        private void PauseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scheduler.PauseAll();
            timer1.Stop();
            BindData();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            StartToolStripMenuItem.Enabled = listView1.SelectedItems.Count != 0;
            PauseToolStripMenuItem.Enabled = listView1.SelectedItems.Count != 0;
            RefreshToolStripMenuItem.Enabled = listView1.Items.Count > 0;

            if (listView1.SelectedItems.Count > 0)
            {
                ITrigger trigger = listView1.SelectedItems[0].Tag as ITrigger;
                if (trigger != null)
                {
                    TriggerState state = scheduler.GetTriggerState(trigger.Key);
                    StartToolStripMenuItem.Enabled = state != TriggerState.Normal;
                    PauseToolStripMenuItem.Enabled = state == TriggerState.Normal;
                }
            }
        }
        #endregion
    }
}

