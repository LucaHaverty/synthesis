﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

partial class ControlGroups : System.Windows.Forms.Form
{

    //Form overrides dispose to clean up the component list.
    [System.Diagnostics.DebuggerNonUserCode()]
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
    }

    //NOTE: The following procedure is required by the Windows Form Designer
    //It can be modified using the Windows Form Designer.  
    //Do not modify it using the code editor.
    [System.Diagnostics.DebuggerStepThrough()]
    private void InitializeComponent()
    {
            this.btnExport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabsMain = new System.Windows.Forms.TabControl();
            this.tabGroups = new System.Windows.Forms.TabPage();
            this.lstGroups = new System.Windows.Forms.ListView();
            this.groups_chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groups_chGrounded = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groups_chFaceColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groups_chHighRes = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groups_chConcavity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabJoints = new System.Windows.Forms.TabPage();
            this.jointPane = new EditorsLibrary.JointEditorPane();
            this.chkHighlightComponents = new System.Windows.Forms.CheckBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabsMain.SuspendLayout();
            this.tabGroups.SuspendLayout();
            this.tabJoints.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(339, 6);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(130, 42);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(12, 687);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 42);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tabsMain
            // 
            this.tabsMain.Controls.Add(this.tabGroups);
            this.tabsMain.Controls.Add(this.tabJoints);
            this.tabsMain.Location = new System.Drawing.Point(13, 13);
            this.tabsMain.Name = "tabsMain";
            this.tabsMain.SelectedIndex = 0;
            this.tabsMain.Size = new System.Drawing.Size(935, 668);
            this.tabsMain.TabIndex = 4;
            this.tabsMain.SelectedIndexChanged += new System.EventHandler(this.tabsMain_SelectedIndexChanged);
            // 
            // tabGroups
            // 
            this.tabGroups.Controls.Add(this.lstGroups);
            this.tabGroups.Location = new System.Drawing.Point(4, 25);
            this.tabGroups.Name = "tabGroups";
            this.tabGroups.Padding = new System.Windows.Forms.Padding(3);
            this.tabGroups.Size = new System.Drawing.Size(927, 639);
            this.tabGroups.TabIndex = 0;
            this.tabGroups.Text = "Object Groups";
            this.tabGroups.UseVisualStyleBackColor = true;
            // 
            // lstGroups
            // 
            this.lstGroups.AutoArrange = false;
            this.lstGroups.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.groups_chName,
            this.groups_chGrounded,
            this.groups_chFaceColor,
            this.groups_chHighRes,
            this.groups_chConcavity});
            this.lstGroups.FullRowSelect = true;
            this.lstGroups.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstGroups.HoverSelection = true;
            this.lstGroups.Location = new System.Drawing.Point(6, 6);
            this.lstGroups.MultiSelect = false;
            this.lstGroups.Name = "lstGroups";
            this.lstGroups.ShowGroups = false;
            this.lstGroups.Size = new System.Drawing.Size(915, 627);
            this.lstGroups.TabIndex = 4;
            this.lstGroups.UseCompatibleStateImageBehavior = false;
            this.lstGroups.View = System.Windows.Forms.View.Details;
            this.lstGroups.SelectedIndexChanged += new System.EventHandler(this.lstGroups_SelectedIndexChanged);
            this.lstGroups.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lstGroups_MouseDoubleClick);
            // 
            // groups_chName
            // 
            this.groups_chName.Text = "Name";
            this.groups_chName.Width = 400;
            // 
            // groups_chGrounded
            // 
            this.groups_chGrounded.Text = "Grounded";
            this.groups_chGrounded.Width = 100;
            // 
            // groups_chFaceColor
            // 
            this.groups_chFaceColor.Text = "Multicolor Parts";
            this.groups_chFaceColor.Width = 120;
            // 
            // groups_chHighRes
            // 
            this.groups_chHighRes.Text = "High Resolution";
            this.groups_chHighRes.Width = 140;
            // 
            // groups_chConcavity
            // 
            this.groups_chConcavity.Text = "Concavity";
            this.groups_chConcavity.Width = 80;
            // 
            // tabJoints
            // 
            this.tabJoints.Controls.Add(this.jointPane);
            this.tabJoints.Location = new System.Drawing.Point(4, 25);
            this.tabJoints.Name = "tabJoints";
            this.tabJoints.Padding = new System.Windows.Forms.Padding(3);
            this.tabJoints.Size = new System.Drawing.Size(927, 639);
            this.tabJoints.TabIndex = 1;
            this.tabJoints.Text = "Joint Options";
            this.tabJoints.UseVisualStyleBackColor = true;
            // 
            // jointPane
            // 
            this.jointPane.Location = new System.Drawing.Point(0, 0);
            this.jointPane.Name = "jointPane";
            this.jointPane.Size = new System.Drawing.Size(927, 639);
            this.jointPane.TabIndex = 0;
            // 
            // chkHighlightComponents
            // 
            this.chkHighlightComponents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkHighlightComponents.AutoSize = true;
            this.chkHighlightComponents.Location = new System.Drawing.Point(171, 699);
            this.chkHighlightComponents.Name = "chkHighlightComponents";
            this.chkHighlightComponents.Size = new System.Drawing.Size(193, 21);
            this.chkHighlightComponents.TabIndex = 5;
            this.chkHighlightComponents.Text = "Highlight Component Sets";
            this.chkHighlightComponents.UseVisualStyleBackColor = true;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Enabled = false;
            this.txtFilePath.Location = new System.Drawing.Point(3, 16);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(201, 22);
            this.txtFilePath.TabIndex = 6;
            this.txtFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(210, 15);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 7;
            this.btnBrowse.Text = "Browse...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtFilePath);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Location = new System.Drawing.Point(475, 687);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(473, 53);
            this.panel1.TabIndex = 8;
            // 
            // ControlGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 741);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkHighlightComponents);
            this.Controls.Add(this.tabsMain);
            this.Controls.Add(this.btnCancel);
            this.MinimumSize = new System.Drawing.Size(800, 300);
            this.Name = "ControlGroups";
            this.Text = "Control Groups";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ControlGroups_FormClosed);
            this.Load += new System.EventHandler(this.ControlGroups_Load);
            this.SizeChanged += new System.EventHandler(this.window_SizeChanged);
            this.tabsMain.ResumeLayout(false);
            this.tabGroups.ResumeLayout(false);
            this.tabJoints.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
    internal System.Windows.Forms.Button btnExport;
    internal System.Windows.Forms.Button btnCancel;

    private System.Windows.Forms.TabControl tabsMain;
    private System.Windows.Forms.TabPage tabGroups;
    private System.Windows.Forms.TabPage tabJoints;
    private System.Windows.Forms.ListView lstGroups;
    private System.Windows.Forms.ColumnHeader groups_chName;
    private System.Windows.Forms.ColumnHeader groups_chFaceColor;
    private System.Windows.Forms.ColumnHeader groups_chHighRes;
    private System.Windows.Forms.ColumnHeader groups_chGrounded;
    private System.Windows.Forms.CheckBox chkHighlightComponents;
    private System.Windows.Forms.ColumnHeader groups_chConcavity;
    private EditorsLibrary.JointEditorPane jointPane;
    private System.Windows.Forms.TextBox txtFilePath;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Panel panel1;
}