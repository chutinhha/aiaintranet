<script type="text/javascript">
    $(function () {
        $("#onetIDListForm select[id$=Lookup], #onetIDListForm select[id$=SelectCandidate]").parents("td.ms-formbody").each(function () {
            var fieldWrapper = $(this);
            var text = fieldWrapper.text();
            var regex = /List=([^;]+);Label=([^;]+)(?:;CT=([0-9a-fx]+))?/i;
            var match = regex.exec(text);
            if (match != null) {
                var list = match[1];
                var label = match[2];
                var ct = match[3];
                var href = list + '/NewForm.aspx?Source=' + escape(location.href);
                if (ct != null && ct.length > 0) {
                    href += '&ContentTypeId=' + ct;
                }

                href = "JavaScript:var options = SP.UI.$create_DialogOptions(); options.url = 'http://techtrainingnotes.blogspot.com'; options.height = 400; void (SP.UI.ModalDialog.showModalDialog(options))";
                var link = '<div style="padding-top: 5px"><img alt="" src="/_layouts/images/rect.gif" style="vertical-align: middle">&nbsp; <a href="' + href + '" onclick="' + href + '">' + label + '</a></div>';
                fieldWrapper.html(fieldWrapper.html().replace(regex, link));
            }
        })
    });
</script>