using System;
using System.Collections.Generic;
using Sgry.Azuki;
using Path = System.IO.Path;
using IHighlighter = Sgry.Azuki.Highlighter.IHighlighter;
using Highlighters = Sgry.Azuki.Highlighter.Highlighters;

namespace Sgry.Ann
{
	class FileType
	{
		#region Fields & Constants
		public const string TextFileTypeName = "Text";
		public const string BatchFileTypeName = "Batch";
		public const string CppFileTypeName = "C/C++";
		public const string CSharpFileTypeName = "C#";
		public const string DiffFileTypeName = "Diff";
		public const string IniFileTypeName = "INI";
		public const string JavaFileTypeName = "Java";
		public const string JavaScriptFileTypeName = "JavaScript";
		public const string LatexFileTypeName = "LaTeX";
		public const string PythonFileTypeName = "Python";
		public const string RubyFileTypeName = "Ruby";
		public const string XmlFileTypeName = "XML";
		static Dictionary<string, FileType> _FileTypeMap
			= new Dictionary<string,FileType>();

		string _Name = null;
		IHighlighter _Highlighter = null;
		AutoIndentHook _AutoIndentHook = null;
		#endregion

		private FileType()
		{}

		static FileType()
		{
			_FileTypeMap.Add( "BatchFileType", BatchFileType );
			_FileTypeMap.Add( "CppFileType", CppFileType );
			_FileTypeMap.Add( "CSharpFileType", CSharpFileType );
			_FileTypeMap.Add( "DiffFileType", DiffFileType );
			_FileTypeMap.Add( "IniFileType", IniFileType );
			_FileTypeMap.Add( "JavaFileType", JavaFileType );
			_FileTypeMap.Add( "JavaScriptFileType", JavaScriptFileType );
			_FileTypeMap.Add( "LatexFileType", LatexFileType );
			_FileTypeMap.Add( "PythonFileType", PythonFileType );
			_FileTypeMap.Add( "RubyFileType", RubyFileType );
			_FileTypeMap.Add( "XmlFileType", XmlFileType );
		}

		#region Factory
		/// <summary>
		/// Gets a new Text file type.
		/// </summary>
		public static FileType TextFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = TextFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new batch file type.
		/// </summary>
		public static FileType BatchFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.BatchFile;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = BatchFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new C/C++ file type.
		/// </summary>
		public static FileType CppFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Cpp;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = CppFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new C# file type.
		/// </summary>
		public static FileType CSharpFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.CSharp;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = CSharpFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Diff / Patch file type.
		/// </summary>
		public static FileType DiffFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Diff;
				fileType._Name = DiffFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new INI file type.
		/// </summary>
		public static FileType IniFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Ini;
				fileType._Name = IniFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Java file type.
		/// </summary>
		public static FileType JavaFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Java;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = JavaFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new JavaScript file type.
		/// </summary>
		public static FileType JavaScriptFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.JavaScript;
				fileType._AutoIndentHook = AutoIndentHooks.CHook;
				fileType._Name = JavaScriptFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new LaTeX file type.
		/// </summary>
		public static FileType LatexFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Latex;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = LatexFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Python file type.
		/// </summary>
		public static FileType PythonFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Python;
				fileType._AutoIndentHook = AutoIndentHooks.PythonHook;
				fileType._Name = PythonFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new Ruby file type.
		/// </summary>
		public static FileType RubyFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Ruby;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = RubyFileTypeName;
				return fileType;
			}
		}

		/// <summary>
		/// Gets a new XML file type.
		/// </summary>
		public static FileType XmlFileType
		{
			get
			{
				FileType fileType = new FileType();
				fileType._Highlighter = Highlighters.Xml;
				fileType._AutoIndentHook = AutoIndentHooks.GenericHook;
				fileType._Name = XmlFileTypeName;
				return fileType;
			}
		}

		public static FileType GetFileTypeByFileName( string fileName )
		{
			string fileExt = Path.GetExtension( fileName );
			if( fileExt == String.Empty )
			{
				return TextFileType;
			}

			foreach( string sectionName in _FileTypeMap.Keys )
			{
				string extList = AppConfig.Ini.Get( sectionName,
													"Extensions",
													"" );
				foreach( string ext in extList.Split(' ') )
				{
					if( String.Compare(fileExt, ext, true) == 0 )
						return _FileTypeMap[sectionName];
				}
			}

			return TextFileType;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Gets an associated highlighter object.
		/// </summary>
		public IHighlighter Highlighter
		{
			get{ return _Highlighter; }
		}

		/// <summary>
		/// Gets key-hook procedure for Azuki's auto-indent associated with this file-type.
		/// </summary>
		public AutoIndentHook AutoIndentHook
		{
			get{ return _AutoIndentHook; }
		}

		/// <summary>
		/// Gets the name of the file mode.
		/// </summary>
		public String Name
		{
			get{ return _Name; }
		}
		#endregion
	}
}
