namespace GActivityDiary.Core.Converters.Text
{
    /// <summary>
    /// Interface for 'text' converters.
    /// </summary>
    /// <typeparam name="T">Other type</typeparam>
    public interface ITextConverter<T>
    {
        /// <summary>
        /// Convert from other type to text (<see cref="string"/>).
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string ToText(T obj);

        /// <summary>
        /// Convert from text (<see cref="string"/>) to other type.
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns></returns>
        T FromText(string text);
    }
}
