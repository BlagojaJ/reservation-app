function splitCamelCase(input: string) {
    return input.replace(/([a-z])([A-Z])/g, '$1 $2')
}

export default splitCamelCase
