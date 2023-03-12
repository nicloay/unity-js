
module.exports.asyncWait = async function (waitTimeMS) {
    console.log('start async process');
    const result = await delay(waitTimeMS); // this function is implemented on Unity Side
    console.log('process finished, returning result result' + result);
}
