function truncateText(text: string, maxLength: number) {
    if (text.length > maxLength) {
        let truncatedText = text.substring(0, maxLength)
        const lastSpaceIndex = truncatedText.lastIndexOf(' ')
        if (lastSpaceIndex > 0) {
            truncatedText = truncatedText.substring(0, lastSpaceIndex)
        }
        return truncatedText + '...'
    } else {
        return text
    }
}
export default truncateText
