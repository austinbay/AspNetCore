// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Runtime.InteropServices;

namespace Microsoft.AspNetCore.Components.RenderTree
{
    /// <summary>
    /// Represents a single edit operation on a component's render tree.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct RenderTreeEdit
    {
        /// <summary>
        /// Gets the type of the edit operation.
        /// </summary>
        [FieldOffset(0)] public readonly RenderTreeEditType Type;

        /// <summary>
        /// Gets the index of the sibling frame that the edit relates to.
        /// </summary>
        [FieldOffset(4)] public readonly int SiblingIndex;

        /// <summary>
        /// Gets the index of related data in an associated render frames array. For example, if the
        /// <see cref="Type"/> value is <see cref="RenderTreeEditType.PrependFrame"/>, gets the
        /// index of the new frame data in an associated render tree.
        /// </summary>
        [FieldOffset(8)] public readonly int ReferenceFrameIndex;

        /// <summary>
        /// If the <see cref="Type"/> value is <see cref="RenderTreeEditType.MoveFrame"/>, get the
        /// sibling index to which the frame should be moved.
        /// </summary>
        [FieldOffset(8)] public readonly int MoveToSiblingIndex;

        /// <summary>
        /// If the <see cref="Type"/> value is <see cref="RenderTreeEditType.RemoveAttribute"/>,
        /// gets the name of the attribute that is being removed.
        /// </summary>
        [FieldOffset(16)] public readonly string RemovedAttributeName;

        private RenderTreeEdit(RenderTreeEditType type) : this()
        {
            Type = type;
        }

        private RenderTreeEdit(RenderTreeEditType type, int siblingIndex) : this()
        {
            Type = type;
            SiblingIndex = siblingIndex;
        }

        private RenderTreeEdit(RenderTreeEditType type, int siblingIndex, int referenceFrameOrMoveToSiblingIndex) : this()
        {
            Type = type;
            SiblingIndex = siblingIndex;

            // MoveToSiblingIndex is stored in the same slot as ReferenceFrameIndex,
            // so assigning to either one is equivalent
            ReferenceFrameIndex = referenceFrameOrMoveToSiblingIndex;
        }

        private RenderTreeEdit(RenderTreeEditType type, int siblingIndex, string removedAttributeName) : this()
        {
            Type = type;
            SiblingIndex = siblingIndex;
            RemovedAttributeName = removedAttributeName;
        }

        internal static RenderTreeEdit RemoveFrame(int siblingIndex)
            => new RenderTreeEdit(RenderTreeEditType.RemoveFrame, siblingIndex);

        internal static RenderTreeEdit PrependFrame(int siblingIndex, int referenceFrameIndex)
            => new RenderTreeEdit(RenderTreeEditType.PrependFrame, siblingIndex, referenceFrameIndex);

        internal static RenderTreeEdit UpdateText(int siblingIndex, int referenceFrameIndex)
            => new RenderTreeEdit(RenderTreeEditType.UpdateText, siblingIndex, referenceFrameIndex);

        internal static RenderTreeEdit UpdateMarkup(int siblingIndex, int referenceFrameIndex)
            => new RenderTreeEdit(RenderTreeEditType.UpdateMarkup, siblingIndex, referenceFrameIndex);

        internal static RenderTreeEdit SetAttribute(int siblingIndex, int referenceFrameIndex)
            => new RenderTreeEdit(RenderTreeEditType.SetAttribute, siblingIndex, referenceFrameIndex);

        internal static RenderTreeEdit RemoveAttribute(int siblingIndex, string name)
            => new RenderTreeEdit(RenderTreeEditType.RemoveAttribute, siblingIndex, name);

        internal static RenderTreeEdit StepIn(int siblingIndex)
            => new RenderTreeEdit(RenderTreeEditType.StepIn, siblingIndex);

        internal static RenderTreeEdit StepOut()
            => new RenderTreeEdit(RenderTreeEditType.StepOut);

        internal static RenderTreeEdit MoveFrame(int siblingIndex, int toSiblingIndex)
            => new RenderTreeEdit(RenderTreeEditType.MoveFrame, siblingIndex, toSiblingIndex);

        internal static RenderTreeEdit EndOfMoveList()
            => new RenderTreeEdit(RenderTreeEditType.EndOfMoveList);
    }
}