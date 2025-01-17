﻿using Hugo_LAND.Client.HugoLandServices;
using Hugo_LAND.Client.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Hugo_LAND.Client.Vue
{
    public partial class frmCreateClass : Form
    {
        private readonly WorldServiceClient worldServiceClient = new WorldServiceClient();
        private readonly ClassServiceClient classServiceClient = new ClassServiceClient();
        private List<WorldDetailsDTO> worldsList;
        private readonly CreateClassValidator createClassValidator;

        public frmCreateClass()
        {
            InitializeComponent();
            worldsList = new List<WorldDetailsDTO>();
            LoadWorlds();
            createClassValidator = new CreateClassValidator();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            List<string> errorsStats = VerifyInfo();
            if (errorsStats.Count > 0)
            {
                ClassDetailsDTO newClass = new ClassDetailsDTO()
                {
                    ClassName = txtName.Text,
                    Description = txtDescription.Text,
                    StatBaseStr = 0,
                    StatBaseDex = 0,
                    StatBaseReg = 0,
                    StatBaseVitality = 0
                };
                var result = createClassValidator.Validate(newClass);
                foreach (var item in result.Errors)
                    errorsStats.Add(item.ErrorMessage);

                MessageBox.Show(string.Join("\n", errorsStats), "Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                ClassDetailsDTO newClass = new ClassDetailsDTO()
                {
                    ClassName = txtName.Text,
                    Description = txtDescription.Text,
                    StatBaseStr = int.Parse(txtStr.Text),
                    StatBaseDex = int.Parse(txtDex.Text),
                    StatBaseReg = int.Parse(txtReg.Text),
                    StatBaseVitality = int.Parse(txtVitality.Text)
                };

                var result = createClassValidator.Validate(newClass);
                WorldDetailsDTO world = worldsList.FirstOrDefault(w => w.Description == comboWorlds.Text);

                if (!result.IsValid)
                {
                    MessageBox.Show(string.Join("\n", result.Errors.ToList()), "Errors", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    bool isSuccess = classServiceClient.CreateClass(newClass, world);
                    if (isSuccess)
                    {
                        MessageBox.Show("The class has been created", "Success!", MessageBoxButtons.OK, MessageBoxIcon.None);
                        this.Close();
                    }
                    else
                        MessageBox.Show("An error has occured with the creation of the class", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadWorlds()
        {
            try
            {
                worldsList = worldServiceClient.GetAllWorldNames().ToList();
                comboWorlds.DataSource = worldsList.Select(w => w.Description).ToList();
            }
            catch
            {
                MessageBox.Show("No worlds have been found or a network error has occured!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescription.Enabled = false;
                txtName.Enabled = false;
                txtStr.Enabled = false;
                txtDex.Enabled = false;
                txtReg.Enabled = false;
                txtVitality.Enabled = false;
                btnCreate.Enabled = false;
                comboWorlds.Enabled = false;
            }
        }
        private List<string> VerifyInfo()
        {
            List<string> errors = new List<string>();
            //Stats
            int StatStr, StatDex, StatReg, StatVitality;
            bool isNumericStr = int.TryParse(txtStr.Text, out StatStr);
            bool isNumericDex = int.TryParse(txtDex.Text, out StatDex);
            bool isNumericReg = int.TryParse(txtReg.Text, out StatReg);
            bool isNumericVitality = int.TryParse(txtVitality.Text, out StatVitality);
            if (!isNumericStr || StatStr < 0 || StatStr > 10)
                errors.Add("Please provide a valid base stat str between(inclusive) 0 and 10.");
            if (!isNumericDex || StatDex < 0 || StatDex > 60)
                errors.Add("Please provide a valid base stat dex between(inclusive) 0 and 60.");
            if (!isNumericReg || StatReg < 0 || StatReg > 5)
                errors.Add("Please provide a valid base stat int between(inclusive) 0 and 5.");
            if (!isNumericVitality || StatVitality < 0 || StatVitality > 200)
                errors.Add("Please provide a valid base stat vitality between(inclusive) 0 and 200.");

            //World
            WorldDetailsDTO w;
            try
            {
                w = worldsList.First(wo => wo.Description == comboWorlds.Text);
            }
            catch
            {
                w = null;
            }
            if (string.IsNullOrEmpty(comboWorlds.Text) || w == null)
                errors.Add("Please select a valid world.");

            return errors;
        }
    }
}
