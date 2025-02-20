export default function formatToCurrency(
    amount: number,
    currency: 'MKD' | 'EUR',
    decimals = 2
) {
    const formatted = amount
        .toFixed(decimals) // Ensure appropriate decimal places
        .replace('.', ',') // Replace the decimal separator with a comma
        .replace(/\B(?=(\d{3})+(?!\d))/g, '.') // Add dots as thousand separators

    // Determine the currency symbol
    let currencySymbol = ''
    if (currency === 'MKD') {
        currencySymbol = 'ден.'
    } else if (currency === 'EUR') {
        currencySymbol = '€'
    }

    // Append the currency symbol
    return `${formatted} ${currencySymbol}`
}
