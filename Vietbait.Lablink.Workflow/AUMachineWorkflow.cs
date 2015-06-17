using System;
using System.Workflow.Activities;

namespace Vietbait.Lablink.Workflow
{
    public sealed partial class AUMachineWorkflow : StateMachineWorkflowActivity
    {
        public AUMachineWorkflow()
        {
            InitializeComponent();
        }

        private void codeInitWorkflowActivity_ExecuteCode(object sender, EventArgs e)
        {
            Console.WriteLine(@"AU Machine Workflow Init!");
        }
    }
}