using System;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Gems.Server.Protos;
namespace Grpc.Gems.Server.Services
{
	public class PointTypeService
		: Protos.PointTypeService.PointTypeServiceBase
	{
		public PointTypeService()
		{
		}

        public override Task<PointType> CreatePointType(CreatePointTypeRequest request, ServerCallContext context)
        {
            return base.CreatePointType(request, context);
        }

        public override Task<Empty> DeletePointType(DeletePointTypeRequest request, ServerCallContext context)
        {
            return base.DeletePointType(request, context);
        }

        public override Task<Empty> DisablePointType(DisablePointTypeRequest request, ServerCallContext context)
        {
            return base.DisablePointType(request, context);
        }

        public override Task<Empty> EnablePointType(EnablePointTypeRequest request, ServerCallContext context)
        {
            return base.EnablePointType(request, context);
        }

        public override Task<PointType> GetPointType(GetPointTypeRequest request, ServerCallContext context)
        {
            return base.GetPointType(request, context);
        }

        public override Task<ListPointTypesReponse> ListPointTypes(ListPointTypesRequest request, ServerCallContext context)
        {
            return base.ListPointTypes(request, context);
        }

        public override Task<PointType> UpdatePointType(UpdatePointTypeRequest request, ServerCallContext context)
        {
            return base.UpdatePointType(request, context);
        }
    }
}

