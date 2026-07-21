using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WinFormsProject.Models;
using System.Configuration;
namespace WinFormsProject
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private IConfiguration _configuration;
        private string _accessToken = "";
        private Newtonsoft.Json.Linq.JArray _currentDocumentsJsonArray;

        public Form1()
        {
            InitializeComponent();
            SetupConfiguration();

            // Suppress DataError popups
            this.dgvAttachments.DataError += (s, e) => { e.ThrowException = false; e.Cancel = true; };
            this.dgvDocuments.DataError += (s, e) => { e.ThrowException = false; e.Cancel = true; };

            this.dgvDocuments.CellClick += dgvDocuments_CellClick;
            this.Shown += Form1_Shown;
        }

        private void SetupConfiguration()
        {
            // UserSecrets ki bilkul zarurat nahi, direct memory dictionary use karein
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string>
    {
        { "ClientId", "\\yyyyyyyyy" },
        { "ClientSecret", "yyyyyyyyyyyyyyyyyyy" }
    };

            _configuration = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        private async void Form1_Shown(object sender, EventArgs e)
        {
            // OAuth token generation and loan pipeline load
            bool tokenSuccess = await GenerateOAuthTokenAsync();
            if (tokenSuccess)
            {
                await LoadLoansFromPipelineAsync();
            }
        }

        private async Task<bool> GenerateOAuthTokenAsync()
        {
            try
            {
                string apiServer = "https://concept.api.elliemae.com";
                string clientId = "yyyyyyyyyyyyyyy";
                string clientSecret = "yyyyyyyyyyyyyyyyyyy";
                string instanceId = "yyyyyuyyyyyyyyyyyyyy";
                string grantType = "yyyyyyyyyyyyyy";
                string scope = "yyyyyyyyyy";

                string tokenUrl = $"{apiServer.TrimEnd('/')}/oauth2/v1/token";

                var tokenRequestData = new Dictionary<string, string>
                {
                    { "grant_type", grantType },
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "instance_id", instanceId },
                    { "scope", scope }
                };

                var requestContent = new FormUrlEncodedContent(tokenRequestData);

                _httpClient.DefaultRequestHeaders.Clear();

                HttpResponseMessage response = await _httpClient.PostAsync(tokenUrl, requestContent);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic tokenData = JsonConvert.DeserializeObject(jsonResponse);
                    _accessToken = tokenData.access_token;
                    //MessageBox.Show($"Token successfully generated!\nToken: {_accessToken.Substring(0, Math.Min(15, _accessToken.Length))}...", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show($"Token Error ({response.StatusCode}): {jsonResponse}", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An exception occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task LoadLoansFromPipelineAsync()
        {
            try
            {
                string apiServer = "https://concept.api.elliemae.com";
                string apiUrl = $"{apiServer.TrimEnd('/')}/encompass/v3/loanPipeline?start=0&limit=1000";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                var pipelineQuery = new
                {
                    fields = new[]
                    {
                        "Loan.LoanFolder",
                        "Fields.4000",
                        "Loan.LoanNumber",
                        "Loan.LoanRate",
                        "Lzoan.LoanAmount",
                        "Loan.LastModified",
                        "Loan.BorrowerName"
                    },
                    sortOrder = new[]
                    {
                        new
                        {
                            canonicalName = "Loan.LastModified",
                            order = "Descending"
                        }
                    },
                    filter = new
                    {
                        canonicalName = "Loan.LoanNumber",
                        matchType = "isNotEmpty"
                    }
                };

                string jsonPayload = JsonConvert.SerializeObject(pipelineQuery);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var loansList = new List<LoanModel>();

                    Newtonsoft.Json.Linq.JArray itemsArray = Newtonsoft.Json.Linq.JArray.Parse(jsonResponse);

                    foreach (var item in itemsArray)
                    {
                        string id = item["loanId"]?.ToString();
                        var fields = item["fields"];

                        string borrower = fields?["Loan.BorrowerName"]?.ToString() ?? "";
                        string amount = fields?["Loan.LoanAmount"]?.ToString() ?? "0";

                        loansList.Add(new LoanModel
                        {
                            LoanId = string.IsNullOrEmpty(id) ? "N/A" : id,
                            BorrowerName = string.IsNullOrEmpty(borrower) ? "N/A" : borrower,
                            LoanAmount = string.IsNullOrEmpty(amount) ? "0" : amount
                        });
                    }

                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            dgvLoans.AutoGenerateColumns = false;
                            dgvLoans.DataSource = null;
                            dgvLoans.DataSource = new System.ComponentModel.BindingList<LoanModel>(loansList);
                        });
                    }
                }
                else
                {
                    string errBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Loans API Error ({response.StatusCode}): {errBody}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An exception occurred while loading loans: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDocumentsForLoanAsync(string loanId)
        {
            try
            {

                string apiServer = "https://concept.api.elliemae.com";
                string apiUrl = $"{apiServer.TrimEnd('/')}/encompass/v1/loans/{loanId}/documents";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var docList = new List<DocumentModel>();

                    _currentDocumentsJsonArray = Newtonsoft.Json.Linq.JArray.Parse(jsonResponse);

                    foreach (var item in _currentDocumentsJsonArray)
                    {
                        string docId = item["documentContractId"]?.ToString()
                                    ?? item["documentId"]?.ToString()
                                    ?? item["id"]?.ToString()
                                    ?? "";

                        docList.Add(new DocumentModel
                        {
                            DocumentId = docId,
                            DocumentName = item["title"]?.ToString() ?? ""
                        });
                    }

                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            dgvDocuments.AutoGenerateColumns = false;

                            if (dgvDocuments.Columns.Count >= 2)
                            {
                                dgvDocuments.Columns[0].DataPropertyName = "DocumentId";
                                dgvDocuments.Columns[1].DataPropertyName = "DocumentName";
                            }

                            dgvDocuments.DataSource = null;
                            dgvDocuments.DataSource = new System.ComponentModel.BindingList<DocumentModel>(docList);
                        });
                    }
                }
                else
                {
                    string errBody = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Documents API Error ({response.StatusCode}): {errBody}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An exception occurred while loading documents: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDocuments_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (_currentDocumentsJsonArray == null || _currentDocumentsJsonArray.Count == 0)
            {
                MessageBox.Show("Document data is not available in memory. Please select the loan again.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow row = dgvDocuments.Rows[e.RowIndex];

            string selectedDocId = "";
            if (row.DataBoundItem is DocumentModel doc)
            {
                selectedDocId = doc.DocumentId;
            }
            else if (row.Cells.Count > 0 && row.Cells[0].Value != null)
            {
                selectedDocId = row.Cells[0].Value.ToString();
            }

            if (string.IsNullOrEmpty(selectedDocId))
            {
                MessageBox.Show("The selected Document ID is empty.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var matchedDoc = _currentDocumentsJsonArray.FirstOrDefault(x =>
                x["documentContractId"]?.ToString() == selectedDocId ||
                x["documentId"]?.ToString() == selectedDocId ||
                x["id"]?.ToString() == selectedDocId
            );

            if (matchedDoc == null)
            {
                MessageBox.Show($"Document with ID '{selectedDocId}' was not found in the API response.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var attachmentList = new List<AttachmentModel>();

            if (matchedDoc["attachments"] != null && matchedDoc["attachments"].HasValues)
            {
                foreach (var att in matchedDoc["attachments"])
                {
                    string entityId = att["entityId"]?.ToString() ?? "";
                    string entityName = att["entityName"]?.ToString() ?? entityId;
                    string ext = Path.GetExtension(entityName);
                    if (string.IsNullOrEmpty(ext)) ext = Path.GetExtension(entityId);
                    ext = ext.Replace(".", "").ToUpper();

                    attachmentList.Add(new AttachmentModel
                    {
                        IsSelected = false,
                        AttachmentId = entityId,
                        AttachmentName = string.IsNullOrEmpty(entityName) ? entityId : entityName,
                        FileType = string.IsNullOrEmpty(ext) ? "FILE" : ext
                    });
                }
            }

            dgvAttachments.DataSource = null;

            if (attachmentList.Count == 0)
            {
                MessageBox.Show("There are no attachments available for the selected document.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvAttachments.AutoGenerateColumns = false;
            if (dgvAttachments.Columns.Count >= 3)
            {
                dgvAttachments.Columns[0].DataPropertyName = "IsSelected";
                dgvAttachments.Columns[1].DataPropertyName = "AttachmentName";
                dgvAttachments.Columns[2].DataPropertyName = "FileType";
            }

            dgvAttachments.DataSource = new System.ComponentModel.BindingList<AttachmentModel>(attachmentList);
        }

        private async void btnDownloadSelected_Click(object sender, EventArgs e)
        {
            string loanId = txtLoanId.Text;
            if (string.IsNullOrEmpty(loanId) || loanId == "N/A")
            {
                MessageBox.Show("Please select a valid loan first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedDocId = "";
            if (dgvDocuments.CurrentRow != null)
            {
                if (dgvDocuments.CurrentRow.DataBoundItem is DocumentModel doc)
                {
                    selectedDocId = doc.DocumentId;
                }
                else if (dgvDocuments.CurrentRow.Cells.Count > 0 && dgvDocuments.CurrentRow.Cells[0].Value != null)
                {
                    selectedDocId = dgvDocuments.CurrentRow.Cells[0].Value.ToString();
                }
            }

            if (string.IsNullOrEmpty(selectedDocId))
            {
                MessageBox.Show("Please select a document first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedList = new List<AttachmentModel>();
            if (dgvAttachments.DataSource is System.ComponentModel.BindingList<AttachmentModel> list)
            {
                selectedList = list.Where(x => x.IsSelected).ToList();
            }

            if (selectedList.Count == 0)
            {
                MessageBox.Show("Please select (check) at least one attachment to download.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                string basePath = @"C:\Users\YousufKhan\Downloads\Yousuf";

                string safeDocFolderName = string.Join("_", selectedDocId.Split(Path.GetInvalidFileNameChars()));
                string targetDirectory = Path.Combine(basePath, safeDocFolderName);

                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                int successCount = 0;

                foreach (var att in selectedList)
                {
                    bool success = await DownloadAttachmentFileAsync(loanId, att.AttachmentId, att.AttachmentName, targetDirectory);
                    if (success) successCount++;
                }

                MessageBox.Show($"{successCount} of {selectedList.Count} files successfully downloaded.\nPath: {targetDirectory}", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during directory creation or file download: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> DownloadAttachmentFileAsync(string loanId, string attachmentId, string fileName, string targetFolder)
        {
            try
            {
                if (!Directory.Exists(targetFolder))
                {
                    Directory.CreateDirectory(targetFolder);
                }

                string safeFileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                string fullPath = Path.Combine(targetFolder, safeFileName);

                // Token Clean Up
                string cleanToken = _accessToken ?? "";
                if (cleanToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    cleanToken = cleanToken.Substring(7).Trim();
                }

                string apiServer = "https://concept.api.elliemae.com";
                string apiUrl = $"{apiServer.TrimEnd('/')}/encompass/v3/loans/{loanId}/attachmentDownloadUrl";

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", cleanToken);

                    var requestPayload = new
                    {
                        attachments = new[] { attachmentId }
                    };

                    string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(requestPayload);
                    var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string jsonString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Download URL API Error ({response.StatusCode}):\n{jsonString}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    // STEP 1: Parse JSON for 'attachments[0].url' and 'authorizationHeader'
                    Newtonsoft.Json.Linq.JObject jsonDoc = Newtonsoft.Json.Linq.JObject.Parse(jsonString);
                    var firstAttachment = jsonDoc["attachments"]?[0];

                    string downloadUrl = firstAttachment?["url"]?.ToString();
                    string authHeader = firstAttachment?["authorizationHeader"]?.ToString();

                    if (string.IsNullOrEmpty(downloadUrl))
                    {
                        MessageBox.Show($"Response me download URL nahi mila!\n\nJSON Content:\n{jsonString}", "Missing URL", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    // STEP 2: Download PDF using 'authorizationHeader' (elli-signature)
                    using (var streamClient = new HttpClient())
                    {
                        if (!string.IsNullOrEmpty(authHeader))
                        {
                            // Streaming server requires elli-signature header
                            streamClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authHeader);
                        }

                        HttpResponseMessage fileResponse = await streamClient.GetAsync(downloadUrl);

                        if (!fileResponse.IsSuccessStatusCode)
                        {
                            string fileErr = await fileResponse.Content.ReadAsStringAsync();
                            MessageBox.Show($"File Stream Download Failed ({fileResponse.StatusCode}):\n{fileErr}", "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                        byte[] pdfBytes = await fileResponse.Content.ReadAsByteArrayAsync();

                        // Write valid binary PDF to disk
                        File.WriteAllBytes(fullPath, pdfBytes);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception during download ({fileName}): {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) { }
        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e) { }

        private async void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLoans.Rows[e.RowIndex];

                if (row.DataBoundItem is LoanModel selectedLoan)
                {
                    txtLoanId.Text = selectedLoan.LoanId;
                    txtBorrowerName.Text = selectedLoan.BorrowerName;

                    dgvAttachments.DataSource = null;

                    if (!string.IsNullOrEmpty(selectedLoan.LoanId) && selectedLoan.LoanId != "N/A")
                    {
                        await LoadDocumentsForLoanAsync(selectedLoan.LoanId);
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void dgvAttachments_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void splitContainer1_Panel1_Paint_1(object sender, PaintEventArgs e) { }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            btnDownloadSelected_Click(sender, e);
        }
    }
}