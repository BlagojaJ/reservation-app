function formatDateString(inputDateString: string): string {
    const date = new Date(inputDateString)

    if (isNaN(date.getTime())) {
        return 'Invalid Date'
    }

    const day = date.getDate().toString().padStart(2, '0')
    const month = (date.getMonth() + 1).toString().padStart(2, '0')
    const year = date.getFullYear().toString().slice(-2)

    const formattedDate = `${day}.${month}.${year}`

    return formattedDate
}

export default formatDateString
