function adjustsTimeZoneOffset(originalDate: Date) {
    const timezoneOffset = originalDate.getTimezoneOffset()

    const adjustedDate = new Date(
        originalDate.getTime() - timezoneOffset * 60000
    )

    return adjustedDate
}

export default adjustsTimeZoneOffset
