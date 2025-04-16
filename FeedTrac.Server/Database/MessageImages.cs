using System.ComponentModel.DataAnnotations;

namespace FeedTrac.Server.Database
{
    /// <summary>
    /// An image object that can be attached to a message
    /// </summary>
    public class MessageImages
    {
        /// <summary>
        /// Id of the image
        /// </summary>
        [Required]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Id of the attached message
        /// </summary>
        public int MessageId { get; set; }

        /// <summary>
        /// Message object
        /// </summary>
        public FeedbackMessage Message { get; set; } = null!;

        /// <summary>
        /// Image data
        /// </summary>
        public byte[] ImageData { get; set; } = null!;

        /// <summary>
        /// Name of the image, alt text
        /// </summary>
        public string ImageName { get; set; } = null!;

        /// <summary>
        /// Mime type the image should be served as
        /// </summary>
        public string ImageType { get; set; } = null!; // MIME type
    }
}
