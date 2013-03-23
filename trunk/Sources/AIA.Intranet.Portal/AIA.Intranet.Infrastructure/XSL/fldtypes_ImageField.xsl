<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" exclude-result-prefixes="xsl" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output indent="yes" method="html" />
  <xsl:template match="FieldRef[@FieldType='ImageField']" mode="Text_body">
    <xsl:param name="thisNode" select="."/>
    <xsl:param name="PictureUrl" select="$thisNode/@*[name()=current()/@Name]" />
    <xsl:if test="$PictureUrl != ''">
      <img>
        <xsl:attribute name="src">
          <xsl:value-of select="substring-after(substring-after($PictureUrl, ';#'), ';#')" />
        </xsl:attribute>
        <xsl:attribute name="style">
          <xsl:text>border-width:0px;</xsl:text>
        </xsl:attribute>
      </img>
    </xsl:if>
  </xsl:template>
</xsl:stylesheet>