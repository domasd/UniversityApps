<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
  <xsl:output method="html" encoding="utf-8" indent="yes"/>

  <xsl:template match="/">
    <html>
      <head>
        <meta charset="utf-8"/>

        <title>Oro Uostai</title>

        <meta name="description" content="Oro uostu xml vizualine reprezentacija"/>
        <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
        <link href="..\..\Content\semantic.css" rel="stylesheet" media="all"/>
        <link href="http://semantic-ui.com/dist/components/icon.css" rel="stylesheet"/>

      </head>
      <body>
        <h1 class="ui center aligned icon header">
          <i class="circular send icon"></i>
          Skrydžiai vykdomi iš šių oro uostų:
        </h1>
        <div class="ui two column doubling stackable  center aligned grid container">
          <!--1 reikalavimas-->
          <!--6 reikalavimas-->
          <xsl:for-each select="OroUostai/OroUostas">
            <!--2 reikalavimas-->
            <xsl:sort select ="@pavadinimas"/>
            <div class="column">
              <h2>
                <!--6 reikalavimas-->
                <!--7 reikalavimas-->
                <xsl:value-of select="@pavadinimas" />
              </h2>

              <!--4 reikalavimas-->
              <!--ima einamojo mazgo visus vaikus (t.y. visus skrydzius)-->
              <xsl:apply-templates/>

            </div>
          </xsl:for-each>
        </div>


      </body>
    </html>
  </xsl:template>

  <xsl:template match="skrydis">
    <br/>

    <h3 class="ui header">
      Skrydis į: <xsl:value-of select="tikslas/@Kodas"/> <br/>
      <div class="sub header">
        <!--3 reikalavimas-->
        ID: <xsl:number count="skrydis" level="any" format="001"/>
      </div>
    </h3>



    <xsl:variable name="pakilimoData" select="data/pakilimo"/>
    <xsl:variable name="siandienData">20151112</xsl:variable>
    
    <!--6 reikalavimas-->
    <xsl:if test ="translate($pakilimoData, '- ', '') &lt; $siandienData">
      <a class="ui red tag label">Įvykęs</a>
    </xsl:if>
    <xsl:if test ="translate($pakilimoData, '- ', '') &gt; $siandienData">
      <a class="ui green tag label">Dar neįvyko</a>

    </xsl:if>

    <table border="1" style="margin:0px auto;">
      <tr>
        <th>
          Informacija
        </th>
        <th>
          Reikšmė
        </th>
      </tr>
      <tr>
        <td>Pakilimo data</td>
        <td>
          <!--5 reikalavimas-->
          <xsl:apply-templates select="data/pakilimo"/>
        </td>
      </tr>
      <tr>
        <td>Pakilimo laikas</td>
        <td>
          <!--5 reikalavimas-->
          <xsl:apply-templates select="laikas/pakilimo"/>
        </td>
      </tr>
    </table>
    <div class="ui divider"></div>
  </xsl:template>

  <xsl:template match="pakilimo">
    <xsl:value-of select="."/>
  </xsl:template>
</xsl:stylesheet>
