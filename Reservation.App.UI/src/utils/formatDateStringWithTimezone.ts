function formatDateStringWithTimezone(inputDateString: string): string {
    const date = new Date(inputDateString)

    if (isNaN(date.getTime())) {
        return 'Invalid Date'
    }

    const day = date.getDate().toString().padStart(2, '0')
    const month = (date.getMonth() + 1).toString().padStart(2, '0')
    const year = date.getFullYear().toString()
    const hours = date.getHours().toString().padStart(2, '0')
    const minutes = date.getMinutes().toString().padStart(2, '0')
    const seconds = date.getSeconds().toString().padStart(2, '0')

    const formattedDate = `${day}.${month}.${year} ${hours}:${minutes}:${seconds}`

    return formattedDate
}

export default formatDateStringWithTimezone
