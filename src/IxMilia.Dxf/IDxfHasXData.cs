using System.Collections.Generic;

namespace IxMilia.Dxf
{
    public interface IDxfHasXData
    {
        IList<DxfCodePairGroup> ExtensionDataGroups { get; }
        DxfXData XData { get; set; }
    }

    internal static class DxfXDataHelper
    {
        public static bool TrySetExtensionData<THasXData>(this THasXData hasXData, DxfCodePair pair, DxfCodePairBufferReader buffer)
            where THasXData : IDxfHasXData
        {
            if (pair.Code == DxfCodePairGroup.GroupCodeNumber && pair.StringValue.StartsWith("{"))
            {
                buffer.Advance();
                var groupName = DxfCodePairGroup.GetGroupName(pair.StringValue);
                hasXData.ExtensionDataGroups.Add(DxfCodePairGroup.FromBuffer(buffer, groupName));
                return true;
            }
            else if (pair.Code == (int)DxfXDataType.ApplicationName)
            {
                buffer.Advance();
                hasXData.XData = DxfXData.FromBuffer(buffer, pair.StringValue);
                return true;
            }

            return false;
        }
    }
}
