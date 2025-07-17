using Markdig;
using System.IO;
using System.Windows.Controls;

namespace SQLGen.ReleaseNotes;

/// <summary>
/// Interaction logic for ReleaseNotesDialog.xaml
/// </summary>
public partial class ReleaseNotesDialog : UserControl
{
	public ReleaseNotesDialog()
	{
		InitializeComponent();
		InitializeAsync();
	}

	async void InitializeAsync()
	{
		await webView.EnsureCoreWebView2Async();

		var mdText = File.ReadAllText(@"ReleaseNotes\ReleaseNotes.md");
		var htmlBody = Markdown.ToHtml(mdText, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());

		var html = $@"
            <!DOCTYPE html>
            <html>
            <head>
              <meta charset=""utf-8"">
              <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/github-markdown-css/github-markdown.css"">
              <style>
                body {{ box-sizing: border-box; margin: 2em; }}
                .markdown-body {{
                  padding: 16px;
                }}
              </style>
            </head>
            <body class=""markdown-body"">
              {htmlBody}
            </body>
            </html>";

		webView.NavigateToString(html);
	}
}
