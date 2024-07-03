using CommunityToolkit.Mvvm.ComponentModel;
using System.Data;

namespace SQLGen.ViewModels;

public partial class SqlDataType : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(HasLength))]
	[NotifyPropertyChangedFor(nameof(HasPrecision))]
	private SqlDbType _type;

	[ObservableProperty]
	private int _length;

	[ObservableProperty]
	private int _precision;

	//Todo add missing types
	private static readonly List<SqlDbType> _typesWithLength = [SqlDbType.Decimal, SqlDbType.Float, SqlDbType.NVarChar];

	//Todo add missing types
	private static readonly List<SqlDbType> _typesWithPrecision = [SqlDbType.Decimal, SqlDbType.Float];

	public bool HasLength => _typesWithLength.Contains(this.Type);
	public bool HasPrecision => _typesWithPrecision.Contains(this.Type);
}