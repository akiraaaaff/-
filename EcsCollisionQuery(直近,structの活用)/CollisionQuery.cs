using P001.Common;
using Unity.Collections;
using Unity.Mathematics;
using static Unity.Mathematics.math;
using float2 = Unity.Mathematics.float2;
using static P001.Simulation.Collision.ShapeTestUtil;
using P001.Simulation.Tree;
using Unity.Entities;
using P001.Simulation.ORCA;
using System.Runtime.CompilerServices;

namespace P001.Simulation.Collision {

    public struct CollisionQuery<TComponent> where TComponent : unmanaged, ITreeComponent {

        public NativeArray<TComponent>.ReadOnly inputComponents;
        public NativeArray<TreeNode>.ReadOnly inputTree;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Query<TQuery>(in ShapeComponent shape, in TQuery queryCallBack)
            where TQuery : IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) { return; }
            bool doNext = true;
            float shapeRadius = shape.Blob.Value.Radius * shape.Scale;

            switch (shape.Blob.Value.Shape) {
                default:
                case Shape.Circle:
                    CircleTestCallBack circleTestCallBack;
                    QueryKDTreeRecursive(shape.Position + shape.Blob.Value.Offset, shapeRadius, shape.Blob.Value.LayerDetection, ref doNext, in circleTestCallBack, in queryCallBack, 0);
                    break;
                case Shape.OBB:
                    var positionOBB = shape.Position + shape.Blob.Value.Offset * shape.Scale * shape.Forward;
                    OBBTestCallBack obbTestCallBack = new(
                        positionOBB,
                        shape.Blob.Value.HalfExtents * shape.Scale,
                        shape.Forward);
                    QueryKDTreeRecursive(positionOBB, shapeRadius, shape.Blob.Value.LayerDetection, ref doNext, in obbTestCallBack, in queryCallBack, 0);
                    break;
                case Shape.Polygon:
                    float2x2 rotationMatrix = new(shape.Forward.x, -shape.Forward.y, shape.Forward.y, shape.Forward.x);
                    var positionPolygon = shape.Position + shape.Blob.Value.Offset * shape.Scale * shape.Forward;
                    NativeArray<float2> points = new(shape.Blob.Value.Points.Length, Allocator.Temp);
                    for (int i = 0; i < points.Length; i++) {
                        points[i] = positionPolygon + mul(rotationMatrix, shape.Blob.Value.Points[i] * shape.Scale);
                    }
                    PolygonTestCallBack polygonTestCallBack = new(in points);
                    QueryKDTreeRecursive(shape.Position, shapeRadius, shape.Blob.Value.LayerDetection, ref doNext, in polygonTestCallBack, in queryCallBack, 0);
                    break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Query<TQuery>(in AgentComponent agent, in TQuery queryCallBack)
            where TQuery : IQueryCallBack<TComponent> {
            QueryCircle(agent.Position, agent.Blob.Value.Radius, agent.Blob.Value.LayerDetection, in queryCallBack);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryList(in ShapeComponent shape) {

            if (inputComponents.Length == 0) { return default; }
            NoneQueryCallBack<TComponent> noneQueryCallBack = new();
            ResultListQueryCallBack<TComponent, NoneQueryCallBack<TComponent>> resultListQueryCallBack = new(noneQueryCallBack);
            Query(in shape, in resultListQueryCallBack);
            return resultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryList<TQuery>(in ShapeComponent shape, in TQuery queryCallBack)
            where TQuery : unmanaged, IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) { return default; }
            ResultListQueryCallBack<TComponent, TQuery> resultListQueryCallBack = new(in queryCallBack);
            Query(in shape, in resultListQueryCallBack);
            return resultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void QueryCircle<TQuery>(float2 position, float radius, Layer layerDetection, in TQuery queryCallBack)
            where TQuery : IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) { return; }
            bool doNext = true;
            CircleTestCallBack circleTestCallBack;
            QueryKDTreeRecursive(position, radius, layerDetection, ref doNext, in circleTestCallBack, in queryCallBack, 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryList(float2 position, float radius, Layer layerDetection) {

            if (inputComponents.Length == 0) { return default; }
            NoneQueryCallBack<TComponent> noneQueryCallBack = new();
            ResultListQueryCallBack<TComponent, NoneQueryCallBack<TComponent>> resultListQueryCallBack = new(noneQueryCallBack);
            QueryCircle(position, radius, layerDetection, in resultListQueryCallBack);
            return resultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryList<TQuery>(float2 position, float radius, Layer layerDetection, in TQuery queryCallBack)
            where TQuery : unmanaged, IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) { return default; }
            ResultListQueryCallBack<TComponent, TQuery> resultListQueryCallBack = new(in queryCallBack);
            QueryCircle(position, radius, layerDetection, in resultListQueryCallBack);
            return resultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryClosestList(float2 position, float radius, Layer layerDetection, int length) {

            if (inputComponents.Length == 0) { return default; }
            NoneQueryCallBack<TComponent> noneQueryCallBack = new();
            ClosestResultListQueryCallBack<TComponent, NoneQueryCallBack<TComponent>> closestResultListQueryCallBack = new(in noneQueryCallBack, length);
            QueryCircle(position, radius, layerDetection, in closestResultListQueryCallBack);
            return closestResultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NativeArray<TComponent>.ReadOnly QueryClosestList<TQuery>(float2 position, float radius, Layer layerDetection, int length, in TQuery queryCallBack)
            where TQuery : unmanaged, IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) { return default; }
            ClosestResultListQueryCallBack<TComponent, TQuery> closestResultListQueryCallBack = new(in queryCallBack, length);
            QueryCircle(position, radius, layerDetection, in closestResultListQueryCallBack);
            return closestResultListQueryCallBack.GetResults();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool QueryClosest(float2 position, float radius, Layer layerDetection, out TComponent result) {

            if (inputComponents.Length == 0) {
                result = default;
                return false;
            }
            NoneQueryCallBack<TComponent> noneQueryCallBack = new();
            ClosestResultQueryCallBack<TComponent, NoneQueryCallBack<TComponent>> closestResultQueryCallBack = new(in noneQueryCallBack);
            QueryCircle(position, radius, layerDetection, in closestResultQueryCallBack);
            return closestResultQueryCallBack.TryGetResults(out result);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool QueryClosest<TQuery>(float2 position, float radius, Layer layerDetection, in TQuery queryCallBack, out TComponent result)
            where TQuery : unmanaged, IQueryCallBack<TComponent> {

            if (inputComponents.Length == 0) {
                result = default;
                return false;
            }
            ClosestResultQueryCallBack<TComponent, TQuery> closestResultQueryCallBack = new(in queryCallBack);
            QueryCircle(position, radius, layerDetection, in closestResultQueryCallBack);
            return closestResultQueryCallBack.TryGetResults(out result);
        }

        private void QueryKDTreeRecursive<TTest, TQuery>(float2 position, float radius, Layer layerDetection, ref bool doNext, in TTest shapeTestCallBack, in TQuery queryCallBack, int node)
            where TTest : IShapeTestCallBack where TQuery : IQueryCallBack<TComponent> {

            TreeNode treeNode = inputTree[node];

            if (treeNode.End - treeNode.Begin <= TreeNode.AgentMaxLeafSize) {
                TComponent c;
                for (int i = treeNode.Begin; i < treeNode.End; ++i) {
                    c = inputComponents[i];

                    if ((layerDetection & c.LayerOccupation) != c.LayerOccupation) {
                        continue;
                    }

                    float distSq = lengthsq(position - c.Position);
                    if (distSq < lengthsq(radius + c.Radius)
                        && shapeTestCallBack.Invoke(c.Position, c.Radius)) {
                        doNext = queryCallBack.Invoke(c, distSq);
                        if (!doNext) { return; }
                    }
                }
            }
            else {
                TreeNode leftNode = inputTree[treeNode.Left], rightNode = inputTree[treeNode.Right];

                float leftRangeSq = lengthsq(radius + leftNode.MaxRadius),
                      rightRangeSq = lengthsq(radius + rightNode.MaxRadius),

                      distSqLeft = lengthsq(max(float2.zero, leftNode.Min - position))
                    + lengthsq(max(float2.zero, position - leftNode.Max)),
                      distSqRight = lengthsq(max(float2.zero, rightNode.Min - position))
                    + lengthsq(max(float2.zero, position - rightNode.Max));

                if (distSqLeft < distSqRight) {
                    if (distSqLeft < leftRangeSq) {
                        QueryKDTreeRecursive(position, radius, layerDetection, ref doNext, in shapeTestCallBack, in queryCallBack, treeNode.Left);

                        if (distSqRight < rightRangeSq) {
                            QueryKDTreeRecursive(position, radius, layerDetection, ref doNext, in shapeTestCallBack, in queryCallBack, treeNode.Right);
                        }
                    }
                }
                else {
                    if (distSqRight < rightRangeSq) {
                        QueryKDTreeRecursive(position, radius, layerDetection, ref doNext, in shapeTestCallBack, in queryCallBack, treeNode.Right);

                        if (distSqLeft < leftRangeSq) {
                            QueryKDTreeRecursive(position, radius, layerDetection, ref doNext, in shapeTestCallBack, in queryCallBack, treeNode.Left);
                        }
                    }
                }

            }

        }

        interface IShapeTestCallBack {
            bool Invoke(float2 position, float radius);
        }

        struct CircleTestCallBack : IShapeTestCallBack {
            readonly bool IShapeTestCallBack.Invoke(float2 position, float radius) {
                return true;
            }
        }

        readonly struct OBBTestCallBack : IShapeTestCallBack {

            readonly float2 positionOBB;
            readonly float2 halfExtentsOBB;
            readonly float2 forwardOBB;

            public OBBTestCallBack(float2 positionOBB, float2 halfExtentsOBB, float2 forwardOBB) {
                this.positionOBB = positionOBB;
                this.halfExtentsOBB = halfExtentsOBB;
                this.forwardOBB = forwardOBB;
            }

            readonly bool IShapeTestCallBack.Invoke(float2 position, float radius) {
                return TestCircleOBB(position, radius, positionOBB, halfExtentsOBB, forwardOBB);
            }
        }

        readonly struct PolygonTestCallBack : IShapeTestCallBack {

            readonly NativeArray<float2>.ReadOnly points;

            public PolygonTestCallBack(in NativeArray<float2> points) {
                this.points = points.AsReadOnly();
            }

            readonly bool IShapeTestCallBack.Invoke(float2 position, float radius) {
                return TestCirclePolygon(position, radius, points);
            }
        }

        unsafe struct ClosestResultQueryCallBack<T, TQuery> : IQueryCallBack<T>
            where T : unmanaged, ITreeComponent where TQuery : unmanaged, IQueryCallBack<T> {

            NativeReference<T> result;
            NativeReference<float> distSq;
            readonly TQuery* queryCallBack;

            public ClosestResultQueryCallBack(in TQuery queryCallBack) {
                result = new(Allocator.Temp);
                distSq = new(float.MaxValue, Allocator.Temp);
                fixed (TQuery* p = &queryCallBack) { this.queryCallBack = p; }
            }

            bool IQueryCallBack<T>.Invoke(in T result, float distSq) {

                if (queryCallBack->Invoke(result, distSq)) {
                    if (distSq < this.distSq.Value) {
                        this.distSq.Value = distSq;
                        this.result.Value = result;
                    }
                }
                return true;
            }

            public readonly bool TryGetResults(out T result) {
                result = this.result.Value;
                return result.Entity != Entity.Null;
            }
        }

        unsafe struct ClosestResultListQueryCallBack<T, TQuery> : IQueryCallBack<T>
            where T : unmanaged, ITreeComponent where TQuery : unmanaged, IQueryCallBack<T> {

            NativeList<T> resultList;
            NativeList<float> distSqList;
            readonly int length;
            readonly TQuery* queryCallBack;

            public ClosestResultListQueryCallBack(in TQuery queryCallBack, int length) {
                resultList = new(length, Allocator.Temp);
                distSqList = new(length, Allocator.Temp);
                this.length = length;
                fixed (TQuery* p = &queryCallBack) { this.queryCallBack = p; }
            }

            bool IQueryCallBack<T>.Invoke(in T result, float distSq) {

                if (queryCallBack->Invoke(result, distSq)) {
                    if (resultList.Length < length) {
                        resultList.Add(result);
                        distSqList.Add(distSq);
                    }

                    int j = resultList.Length - 1;
                    if (distSq < distSqList[j]) {

                        while (j != 0 && distSq < distSqList[j - 1]) {
                            resultList[j] = resultList[j - 1];
                            distSqList[j] = distSqList[j - 1];
                            --j;
                        }

                        resultList[j] = result;
                        distSqList[j] = distSq;
                    }
                }

                return true;
            }

            public NativeArray<T>.ReadOnly GetResults() {
                return resultList.AsReadOnly();
            }
        }

        unsafe struct ResultListQueryCallBack<T, TQuery> : IQueryCallBack<T>
            where T : unmanaged, ITreeComponent where TQuery : unmanaged, IQueryCallBack<T> {

            NativeList<T> results;
            readonly TQuery* queryCallBack;

            public ResultListQueryCallBack(in TQuery queryCallBack, int initialCapacity = 1) {
                results = new(initialCapacity, Allocator.Temp);
                fixed (TQuery* p = &queryCallBack) { this.queryCallBack = p; }
            }

            bool IQueryCallBack<T>.Invoke(in T result, float distSq) {
                if (queryCallBack->Invoke(result, distSq)) {
                    results.Add(result);
                }
                return true;
            }

            public NativeArray<T>.ReadOnly GetResults() {
                return results.AsReadOnly();
            }
        }
        struct NoneQueryCallBack<T> : IQueryCallBack<T> where T : ITreeComponent {
            readonly bool IQueryCallBack<T>.Invoke(in T result, float distSq) {
                return true;
            }
        }
    }

    public interface IQueryCallBack<TComponent> where TComponent : ITreeComponent {
        bool Invoke(in TComponent result, float distSq);
    }
}
