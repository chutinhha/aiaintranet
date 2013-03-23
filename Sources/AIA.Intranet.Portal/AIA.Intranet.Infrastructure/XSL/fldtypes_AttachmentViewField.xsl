<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema"
                xmlns:d="http://schemas.microsoft.com/sharepoint/dsp"
                version="1.0"
                exclude-result-prefixes="xsl msxsl ddwrt"
                xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime"
                xmlns:asp="http://schemas.microsoft.com/ASPNET/20"
                xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:SharePoint="Microsoft.SharePoint.WebControls"
                xmlns:ddwrt2="urn:frontpage:internal">

  <xsl:template name="FieldRef_ISABC_body"  match ="FieldRef[@Name='ISABC']" mode="Text_body">
    <xsl:param name="thisNode" select="."/>

    <xsl:variable name="url">
      <xsl:call-template name="EncodedAbsUrl">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="ID">
      <xsl:call-template name="ResolveId">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="urlencode" select="$thisNode/@FileRef.urlencode" />

    <a  href="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={$url}&amp;urlencode={$urlencode}&amp;ID={$ID}&amp;List={$List}" >
      <img src="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={$url}" width="62px" height="62px"/>

    </a>


  </xsl:template>


  <xsl:template name="FieldRef_IAvatar_body"  match ="FieldRef[@Name='IAvatar']" mode="Text_body">
    <xsl:param name="thisNode" select="."/>

    <xsl:variable name="url">
      <xsl:call-template name="EncodedAbsUrl">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="ID">
      <xsl:call-template name="ResolveId">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="urlencode" select="$thisNode/@FileRef.urlencode" />

    <a  href="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={$url}&amp;urlencode={$urlencode}&amp;ID={$ID}&amp;List={$List}" >
      <img src="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={$url}&amp;urlencode={$urlencode}&amp;ID={$ID}&amp;List={$List}" width="62px" height="62px"/>

    </a>


  </xsl:template>

  <!--<xsl:template name="FieldRef_IAvatar_body"  match ="FieldRef[@Name='IAvatar1']" mode="Text_body">
    <xsl:param name="thisNode" select="."/>
    <xsl:variable name="EmployeeName" select="$thisNode/@EmployeeName" />
    <xsl:variable name="url">
      <xsl:call-template name="EncodedAbsUrl">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>

    <xsl:variable name="ID">
      <xsl:call-template name="ResolveId">
        <xsl:with-param name="thisNode" select ="$thisNode"/>
      </xsl:call-template>
    </xsl:variable>
    <xsl:variable name="urlencode" select="$thisNode/@FileRef.urlencode" />

    <a  href="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item=/Lists/Employee&amp;urlencode={$urlencode}&amp;ID={$ID}&amp;List={$List}" >
      <img src="/_layouts/AIA.Intranet.Infrastructure/DisplayAttachments.ashx?item={$url}&amp;urlencode={$urlencode}&amp;ID={$ID}&amp;List={$List}" width="62px" height="62px"/>

    </a>


  </xsl:template>-->
  
</xsl:stylesheet>

