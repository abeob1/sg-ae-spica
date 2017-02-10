﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1008
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("SpicaInterCompany.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to CREATE VIEW [dbo].[ARInvoiceXmlData]
        '''AS
        '''SELECT     T0.DocEntry, ISNULL(T0.DocNum, N&apos;&apos;) AS invoiceNumber, T0.DocDate AS invDate, ISNULL(T0.U_AE_PDateFr, N&apos;&apos;) AS invCoveredFrom, ISNULL(T0.U_AE_PDateTo, N&apos;&apos;) 
        '''                      AS invCoveredTo, ISNULL(T0.U_AE_FinalInv, N&apos;&apos;) AS invFinalInvoice, GETDATE() AS invArchiveDate, T0.CardName AS invtoName, T0.CardCode AS invtoID, 
        '''                      ISNULL(T0.U_AE_Member, N&apos;&apos;) AS invtoTo, ISNULL(T1.Name, N&apos;&apos;) AS invtoContactName, ISNULL(T1.E_MailL, N&apos;&apos;) AS in [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property ARInvoiceXmlData() As String
            Get
                Return ResourceManager.GetString("ARInvoiceXmlData", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Declare @stringXML as nvarchar(max)
        '''Set @stringXML = &apos;&apos;
        '''exec sp_AE_GenerateXML {0}, @stringXML output
        '''
        '''Declare @table as table (Value nvarchar(256))
        '''Declare @StringLen as int
        '''Declare @Cursor as int
        '''set @Cursor = 0
        '''If @stringXML &lt;&gt; &apos;&apos;
        '''Begin
        '''set @StringLen = (select LEN(@stringXML))
        '''	while @Cursor &lt; @StringLen
        '''	Begin
        '''		insert into @table  select substring(@stringXML	, @Cursor, 100)
        '''		Set @Cursor = @Cursor + 100
        '''		
        '''	End;	
        '''End
        '''select Value from @table
        '''.
        '''</summary>
        Friend ReadOnly Property GetXMLString() As String
            Get
                Return ResourceManager.GetString("GetXMLString", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to CREATE PROCEDURE [dbo].[sp_AE_GenerateXML]
        '''@DocEntry numeric(18,0)
        '''as
        '''with invoice as
        '''(
        '''	select Top 1
        '''		invoiceNumber 
        '''		,invDate = CONVERT(char(10), invDate,126)
        '''		,invCoveredFrom = CONVERT(char(10), invCoveredFrom,126)
        '''		,invCoveredTo = CONVERT(char(10), invCoveredTo,126)
        '''		,invFinalInvoice
        '''		,invArchiveDate = CONVERT(char(10), GetDate(),126)
        '''	from 
        '''		ARInvoiceXMLData
        '''	where
        '''		DocEntry = @DocEntry
        '''),
        '''address as
        '''(
        '''	select Top 1
        '''		invtoLine1
        '''		,invtoLine2
        '''		,invtoLine3
        '''		,invtoLine4
        ''' [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property sp_AE_GenerateXML() As String
            Get
                Return ResourceManager.GetString("sp_AE_GenerateXML", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
