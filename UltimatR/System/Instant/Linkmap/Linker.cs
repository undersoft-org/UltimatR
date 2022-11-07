
// <copyright file="Linker.cs" company="UltimatR.Core">
//     Copyright (c) Undersoft. All rights reserved.
// </copyright>



/// <summary>
/// The Linking namespace.
/// </summary>
namespace System.Instant.Linking
{
    using Linq;
    using Series;
    using Uniques;

    /// <summary>
    /// Interface ILinker
    /// </summary>
    public interface ILinker
    {
        /// <summary>
        /// Gets the origin links.
        /// </summary>
        /// <value>The origin links.</value>
        Links OriginLinks { get; }

        /// <summary>
        /// Gets the target links.
        /// </summary>
        /// <value>The target links.</value>
        Links TargetLinks { get; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the origin.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>Link.</returns>
        Link GetOrigin(IFigure target, string OriginName);

        /// <summary>
        /// Gets the origins.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>IDeck&lt;Link&gt;.</returns>
        IDeck<Link> GetOrigins(IFigures target, string OriginName);

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>Link.</returns>
        Link GetTarget(IFigure origin, string TargetName);

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>IDeck&lt;Link&gt;.</returns>
        IDeck<Link> GetTargets(IFigures origin, string TargetName);

    }

    /// <summary>
    /// Class Linker.
    /// </summary>
    [Serializable]
    public class Linker 
    {
        /// <summary>
        /// The map
        /// </summary>
        private static Catalog<Link> map = new Catalog<Link>(true, PRIMES_ARRAY.Get(9));
        /// <summary>
        /// The origin links
        /// </summary>
        private Links originLinks;
        /// <summary>
        /// The target links
        /// </summary>
        private Links targetLinks;

        /// <summary>
        /// Initializes a new instance of the <see cref="Linker" /> class.
        /// </summary>
        public Linker()
        {
            originLinks = new Links();
            targetLinks = new Links();
        }

        /// <summary>
        /// Gets the map.
        /// </summary>
        /// <value>The map.</value>
        public static IDeck<Link> Map { get => map; }

        /// <summary>
        /// Gets or sets the link.
        /// </summary>
        /// <value>The link.</value>
        public Link Link { get; set; }

        /// <summary>
        /// Gets the origin links.
        /// </summary>
        /// <value>The origin links.</value>
        public Links OriginLinks { get => originLinks; }

        /// <summary>
        /// Gets the target links.
        /// </summary>
        /// <value>The target links.</value>
        public Links TargetLinks { get => targetLinks; }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            Map.Flush();
        }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>Link.</returns>
        public Link GetOrigin(ISleeve figure, string OriginName)
        {
            return map[OriginKey(figure, OriginName)];
        }

        /// <summary>
        /// Gets the origin link.
        /// </summary>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>Link.</returns>
        public Link GetOriginLink(string OriginName)
        {
            return OriginLinks[OriginName + "_" + Link.Name];
        }

        /// <summary>
        /// Gets the origin member.
        /// </summary>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>LinkMember.</returns>
        public LinkMember GetOriginMember(string OriginName)
        {
            Link link = GetOriginLink(OriginName);
            if (link != null)
                return link.Origin;
            return null;
        }

        /// <summary>
        /// Gets the origins.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>IDeck&lt;Link&gt;.</returns>
        public IDeck<Link> GetOrigins(Links figures, string OriginName)
        {
            var originMember = GetOriginMember(OriginName);
            return new Album<Link>(figures.Select(f => map[originMember.LinkKey(f.ToSleeve())]), 255);
        }

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>Link.</returns>
        public Link GetTarget(ISleeve figure, string TargetName)
        {
            return map[TargetKey(figure, TargetName)];
        }

        /// <summary>
        /// Gets the target link.
        /// </summary>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>Link.</returns>
        public Link GetTargetLink(string TargetName)
        {
            return TargetLinks[Link.Name + "_&_" + TargetName];
        }

        /// <summary>
        /// Gets the target member.
        /// </summary>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>LinkMember.</returns>
        public LinkMember GetTargetMember(string TargetName)
        {
            Link link = GetTargetLink(TargetName);
            if (link != null)
                return link.Target;
            return null;
        }

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>IDeck&lt;Link&gt;.</returns>
        public IDeck<Link> GetTargets(IFigures figures, string TargetName)
        {
            var targetMember = GetTargetMember(TargetName);
            return new Album<Link>(figures.Select(f => map[targetMember.LinkKey(f.ToSleeve())]).ToArray(), 255);
        }

        /// <summary>
        /// Origins the key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>System.UInt64.</returns>
        public ulong OriginKey(ISleeve figure, string OriginName)
        {
            return GetOriginMember(OriginName).LinkKey(figure);
        }

        /// <summary>
        /// Targets the key.
        /// </summary>
        /// <param name="figure">The figure.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>System.UInt64.</returns>
        public ulong TargetKey(ISleeve figure, string TargetName)
        {
            return GetTargetMember(TargetName).LinkKey(figure);
        }

    }

    /// <summary>
    /// Class LinkerExtension.
    /// </summary>
    public static class LinkerExtension
    {
        /// <summary>
        /// Gets the origin link.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="OriginName">Name of the origin.</param>
        /// <returns>Link.</returns>
        public static Link GetOriginLink(this Sleeve figures, string OriginName)
        {
            return Linker.Map[OriginName + "_" + figures.Name];
        }

        /// <summary>
        /// Gets the target link.
        /// </summary>
        /// <param name="figures">The figures.</param>
        /// <param name="TargetName">Name of the target.</param>
        /// <returns>Link.</returns>
        public static Link GetTargetLink(this Sleeve figures, string TargetName)
        {
            return Linker.Map[figures.Name + "_" + TargetName];
        }

    }
}
