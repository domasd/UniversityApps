<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>
  
  <xsl:template match="/">
    <ol type="1">
      <xsl:for-each select="Knygynas/knygos/grupė/knyga">
        <xsl:sort select ="pavadinimas"/>
        <h2>

          <li>
            <xsl:value-of select="pavadinimas" />
          </li>
        </h2>
      </xsl:for-each>
    </ol>
  </xsl:template>
</xsl:stylesheet>
